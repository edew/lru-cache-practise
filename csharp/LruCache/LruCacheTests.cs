using Xunit;

namespace LruCache
{
    public class LruCacheTests
    {
        [Fact]
        public void Returns_Null_For_Key_Not_In_Cache()
        {
            var cache = new LruCache<string, object>(2);

            Assert.Null(cache.Get("some key"));
        }

        [Fact]
        public void Returns_Value_For_Key_In_Cache()
        {
            var cache = new LruCache<string, string>(1);

            cache.Set("foo", "bar");

            Assert.Equal("bar", cache.Get("foo"));
        }

        [Fact]
        public void Evicts_1()
        {
            var cache = new LruCache<int, string>(1);

            cache.Set(1, "1");
            cache.Set(2, "2");

            Assert.Null(cache.Get(1));
            Assert.Equal("2", cache.Get(2));
        }

        [Fact]
        public void Evicts_2()
        {
            var cache = new LruCache<int, string>(2);

            cache.Set(1, "one");
            cache.Set(2, "two");
            cache.Get(1);
            cache.Set(3, "three");

            Assert.Null(cache.Get(2));
            Assert.Equal("one", cache.Get(1));
            Assert.Equal("three", cache.Get(3));
        }

        [Fact]
        public void Evicts_3_And_5()
        {
            var cache = new LruCache<int, int>(5);

            cache.Set(1, 1);
            cache.Set(2, 2);
            cache.Set(3, 3);
            cache.Set(4, 4);
            cache.Set(5, 5);
            cache.Get(1);
            cache.Get(2);
            cache.Get(4);
            cache.Set(6, 6);
            cache.Set(7, 7);

            Assert.Null(cache.Get(3));
            Assert.Null(cache.Get(5));
            Assert.Equal(1, cache.Get(1));
            Assert.Equal(2, cache.Get(2));
            Assert.Equal(4, cache.Get(4));
            Assert.Equal(6, cache.Get(6));
            Assert.Equal(7, cache.Get(7));
        }
    }
}
