using System;

namespace StansAssets.MarkingMenu
{
	internal class LazyInitialization<T> where T : class
	{
		public LazyInitialization(Func<T> initializer)
		{
			m_initializer = initializer;
		}

		public T Value
		{
			get { return m_value ?? (m_value = m_initializer()); }
		}

		public void Refresh()
		{
			m_value = m_initializer();
		}

		private T m_value;
		private readonly Func<T> m_initializer;
	}
}