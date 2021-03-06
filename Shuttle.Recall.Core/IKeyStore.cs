﻿using System;

namespace Shuttle.Recall.Core
{
	public interface IKeyStore
	{
		Guid? Get(string key);
		void Remove(string key);
		void Add(Guid id, string key);
	}
}