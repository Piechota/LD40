using System.Collections.Generic;

public class AEvent
{
	private readonly List<System.Action> m_Actions = new List<System.Action>();

	public void AddListener(System.Action action)
	{
		m_Actions.Add(action);
	}

	public void RemoveListener(System.Action action)
	{
		m_Actions.Remove(action);
	}

	public void Invoke()
	{
		for (int i = 0; i < m_Actions.Count; ++i)
		{
			System.Action action = m_Actions[i];
			if (action != null)
			{
				action();
			}
		}
	}
}

public class AEvent<T>
{
	private readonly List<System.Action<T>> m_Actions = new List<System.Action<T>>();

	public void AddListener(System.Action<T> action)
	{
		m_Actions.Add(action);
	}

	public void RemoveListener(System.Action<T> action)
	{
		m_Actions.Remove(action);
	}

	public void Invoke(T t)
	{
		for (int i = 0; i < m_Actions.Count; ++i)
		{
			System.Action<T> action = m_Actions[i];
			if (action != null)
			{
				action(t);
			}
		}
	}
}