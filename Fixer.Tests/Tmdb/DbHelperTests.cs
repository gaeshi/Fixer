using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Fixer.Data;
using Fixer.Tmdb;
using Fixer.Tmdb.Models;
using Fixer.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fixer.Tests.Tmdb
{
    public class DbHelperTests
    {
        [Fact]
        public void AddOrUpdateMovies_EmptyList_AddNotCalled()
        {
            var mockSet = new Mock<DbSet<Movie>>();
            var mockContext = new Mock<MovieDbContext>();
            var dbHelper = new DbHelper(Mock.Of<ILogger<DbHelper>>(), mockContext.Object, Mock.Of<IDateProvider>());

            dbHelper.AddOrUpdateMovies(ImmutableList<Movie>.Empty, Filter.Popular);

            mockSet.Verify(m => m.Add(It.IsAny<Movie>()), Times.Never);
        }

        [Fact]
        public void AddOrUpdateMovies_InputNotEmpty_AddCalled()
        {
            var data = new List<Movie>().AsQueryable();
            var mockSet = PrepareMockMovieDbSet(data);
            var dbHelper = CreateDbHelperWithMockedDependencies(mockSet);

            dbHelper.AddOrUpdateMovies(new List<Movie>(new[] {new Movie()}), Filter.Popular);

            mockSet.Verify(m => m.Add(It.IsAny<Movie>()), Times.Once);
        }

        [Fact]
        public void AddOrUpdateMovies_WritesToDb()
        {
            var options = CreateDbContextOptions("AddOrUpdateMovies_WritesToDb");
            var movies = new[] {new Movie {Id = 1, Title = "Fake title"}};

            AddMovieListToDb(options, movies, Filter.Popular);

            using (var context = new MovieDbContext(options))
            {
                Assert.Equal(1, context.Movies.Count());
                Assert.Equal(movies[0].Id, context.Movies.Single().Id);
                Assert.Equal(movies[0].Title, context.Movies.Single().Title);
            }
        }

        [Fact]
        public void AddOrUpdateMovies_DuplicateInput_OneItemAdded()
        {
            var options = CreateDbContextOptions("AddOrUpdateMovies_DuplicateInput_OneItemAdded");
            var movies = new[]
            {
                new Movie {Id = 1},
                new Movie {Id = 1},
                new Movie {Id = 1}
            };

            AddMovieListToDb(options, movies, Filter.Popular);

            using (var context = new MovieDbContext(options))
            {
                Assert.Equal(1, context.Movies.Count());
            }
        }

        [Fact]
        public void AddOrUpdateMovies_ThreeDiffItemInput_ThreeItemsAdded()
        {
            var options = CreateDbContextOptions("AddOrUpdateMovies_ThreeDiffItemInput_ThreeItemsAdded");
            var movies = new[]
            {
                new Movie {Id = 1, Title = "one"},
                new Movie {Id = 2, Title = "two"},
                new Movie {Id = 3, Title = "three"},
            };

            AddMovieListToDb(options, movies, Filter.Popular);

            using (var context = new MovieDbContext(options))
            {
                Assert.Equal(3, context.Movies.Count());
                Assert.Equal("one", context.Movies.Find(1L).Title);
                Assert.Equal("two", context.Movies.Find(2L).Title);
                Assert.Equal("three", context.Movies.Find(3L).Title);
            }
        }

        [Fact]
        public void AddOrUpdateMovies_DifferentFilters_FiltersMerged()
        {
            var options = CreateDbContextOptions("AddOrUpdateMovies_DifferentFilters_FiltersMerged");

            AddMovieListToDb(options, new[] {new Movie {Id = 1}}, Filter.Popular);
            AddMovieListToDb(options, new[] {new Movie {Id = 1}}, Filter.TopRated);

            using (var context = new MovieDbContext(options))
            {
                Assert.Equal(1, context.Movies.Count());
                Assert.Equal(Filter.Popular | Filter.TopRated, context.Movies.Single().Filters);
            }
        }

        private static DbContextOptions<MovieDbContext> CreateDbContextOptions(string name)
        {
            return new DbContextOptionsBuilder<MovieDbContext>().UseInMemoryDatabase(name).Options;
        }

        private static void AddMovieListToDb(
            DbContextOptions<MovieDbContext> options, IEnumerable<Movie> movies, Filter filter)
        {
            using (var context = new MovieDbContext(options))
            {
                var dbHelper = new DbHelper(Mock.Of<ILogger<DbHelper>>(), context, Mock.Of<IDateProvider>());
                dbHelper.AddOrUpdateMovies(movies, filter);
            }
        }

        private static DbHelper CreateDbHelperWithMockedDependencies(IMock<DbSet<Movie>> mockSet)
        {
            var mockContext = new Mock<MovieDbContext>();
            mockContext.Setup(c => c.Movies).Returns(mockSet.Object);
            return new DbHelper(Mock.Of<ILogger<DbHelper>>(), mockContext.Object, Mock.Of<IDateProvider>());
        }

        private static Mock<DbSet<Movie>> PrepareMockMovieDbSet(IQueryable<Movie> data)
        {
            var mockSet = new Mock<DbSet<Movie>>();
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Movie>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
    }
}