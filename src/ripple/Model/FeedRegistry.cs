using FubuCore.Util;
using ripple.Nuget;

namespace ripple.Model
{
	public interface IFeedProvider
	{
		INugetFeed For(Feed feed);
	}

	public class FeedProvider : IFeedProvider
	{
		private readonly Cache<Feed, INugetFeed> _feeds;

		public FeedProvider()
		{
			_feeds = new Cache<Feed, INugetFeed>(buildFeed);
		}

		public INugetFeed For(Feed feed)
		{
			return _feeds[feed];
		}

		private INugetFeed buildFeed(Feed feed)
		{
			if (feed.Mode == UpdateMode.Fixed)
			{
				return new NugetFeed(feed.Url);
			}

			return new FloatingFeed(feed.Url);
		}
	}

	public class FeedRegistry
	{
		private static IFeedProvider _provider;

		static FeedRegistry()
		{
			Reset();
		}

		public static void Stub(IFeedProvider provider)
		{
			_provider = provider;
		}

		public static void Reset()
		{
			Stub(new FeedProvider());
		}

	 	public static INugetFeed FeedFor(Feed feed)
	 	{
	 		return _provider.For(feed);
	 	}
	}
}