﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TCMobile.Models
{
	public enum MenuItemType
	{
		RunningBuilds,
		Projects,
		Changes,
		Agents,
		BuildQueue,
		About
	}
	public class HomeMenuItem
	{
		public MenuItemType Id { get; set; }

		public string Title { get; set; }
	}
}
