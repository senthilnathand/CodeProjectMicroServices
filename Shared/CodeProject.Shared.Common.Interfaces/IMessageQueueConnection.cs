﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.Shared.Common.Interfaces
{
	public interface IMessageQueueConnection
	{
		void IncrementCounter(string queueName);
	}

}
