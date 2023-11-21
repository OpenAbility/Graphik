using OpenAbility.Graphik.Selection;

namespace OpenAbility.Graphik
{

	/// <summary>
	/// Utility for selecting an appropriate Graphik API
	/// </summary>
	public static class GraphikAPISelector
	{
		private static readonly List<GraphikAPIProvider> Providers = new List<GraphikAPIProvider>();

		/// <summary>
		/// Register a backend provider
		/// </summary>
		/// <param name="provider">The provider to register</param>
		public static void RegisterProvider(GraphikAPIProvider provider)
		{
			Providers.Add(provider);
		}

		/// <summary>
		/// Perform an automatic judgement call to which Graphik backend should be used on the current platform
		/// </summary>
		/// <param name="request">The request parameters</param>
		/// <returns>The API, or null if none could be found</returns>
		public static IGraphikAPI? CreateSelection(APIRequest request)
		{
			/*
			 * The provider is chosen as follows:
			 * - We check each provider
			 * - If the providers API rating is HIGHER than the current highest we set it to the most valuable.
			 * - If they are equal, the one with the highest priority wins.
			 */
			
			
			APICreator? bestCreator = null;
			ulong bestAPIRating = 0;
			long currentPriority = Int64.MinValue;

			foreach (GraphikAPIProvider provider in Providers)
			{
				ulong providerRating = provider.Rater(request);

				if (bestAPIRating > providerRating)
					continue;

				if (bestAPIRating < providerRating)
				{
					currentPriority = provider.Priority;
					bestAPIRating = providerRating;
					bestCreator = provider.Creator;
					continue;
				}

				if (provider.Priority <= currentPriority)
					continue;

				bestCreator = provider.Creator;
				currentPriority = provider.Priority;
				bestAPIRating = providerRating;
			}

			return bestCreator?.Invoke();
		}

		/// <summary>
		/// Use your own ideas to select the Graphik backend that suits you the best
		/// </summary>
		/// <param name="selector">The selector function</param>
		/// <returns>The preferred Graphik backend, if any is available</returns>
		public static IGraphikAPI? Select(APISelector selector)
		{
			APICreator? bestCreator = null;
			ulong bestAPIRating = 0;

			foreach (GraphikAPIProvider provider in Providers)
			{
				ulong rating = selector(provider.Specifier());

				if (rating <= bestAPIRating)
					continue;

				bestCreator = provider.Creator;
				bestAPIRating = rating;
			}

			return bestCreator?.Invoke();
		}
	}
}

namespace OpenAbility.Graphik.Selection
{
	
	/// <summary>
	/// Function which creates a Graphik backend
	/// </summary>
	public delegate IGraphikAPI APICreator();
	
	/// <summary>
	/// Rates how suited a backend is based on user request. Higher is better.
	/// </summary>
	public delegate ulong APIRater(APIRequest request);
	
	/// <summary>
	/// Gets the specification for a backend
	/// </summary>
	public delegate APISpecification APISpecifier();

	/// <summary>
	/// Rates how suited a specification seems. Higher is better.
	/// </summary>
	public delegate ulong APISelector(APISpecification specification);
	
	/// <summary>
	/// Specifies a number of things about an API provider
	/// </summary>
	public struct GraphikAPIProvider
	{
		public readonly APICreator Creator;
		public readonly APIRater Rater;
		public readonly APISpecifier Specifier;
		public readonly long Priority;
		
		public GraphikAPIProvider(APICreator creator, APIRater rater, APISpecifier specifier, long priority)
		{
			Creator = creator;
			Rater = rater;
			Specifier = specifier;
			Priority = priority;
		}
	}
	
	/// <summary>
	/// API specification, basically just a read only string-string dictionary.
	/// </summary>
	public class APISpecification
	{
		private readonly Dictionary<string, string> underlying;

		public APISpecification(IDictionary<string, string> data)
		{
			underlying = new Dictionary<string, string>(data);
		}

		/// <summary>
		/// Get the value stored at a key
		/// </summary>
		/// <param name="key">The value, or an empty string if the value is not found</param>
		public string this[string key]
		{
			get
			{
				return underlying.TryGetValue(key, out string? value) ? value : "";
			}
		}

		public IEnumerable<string> Keys
		{
			get
			{
				return underlying.Keys;
			}
		}
	}

}

