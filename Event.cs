using System;
using System.Collections.Generic;
using System.Text;

namespace Simulacao_T1
{
    class Event
    {
		private int type;
		private double time;

		public Event(int type, double time)
		{
			this.type = type;
			this.time = time;
		}
		public int getType()
		{
			return type;
		}
		public void setType(int type)
		{
			this.type = type;
		}
		public double getTime()
		{
			return time;
		}
		public void setTime(double time)
		{
			this.time = time;
		}
	}
}
