namespace OpenAbility.Graphik;

public abstract partial class APILayer
{
	protected readonly IGraphikAPI Underlying;
	public APILayer(IGraphikAPI underlying)
	{
		Underlying = underlying;
	}

	protected virtual object? Intercept(string function, Delegate target, params object?[] parameters)
	{
		return target.DynamicInvoke(parameters);
	}

}