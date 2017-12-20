using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyingMetal.Models
{

	public class HomeModel
	{
		public List<BaseModel> HeadMenuLeft { set; get; }
		public List<BaseModel> HeadMenuRight { set; get; }
		public List<BaseModel> Header { set; get; }

		public HomeModel()
		{
			this.HeadMenuLeft = new List<BaseModel>();
			this.HeadMenuRight = new List<BaseModel>();
			this.Header = new List<BaseModel>();
		}
	}


	public class BaseModel
	{
		public int Id { set; get; }
		public string Body { set; get; }
		public string Styles { set; get; }
		public string Class { set; get; }
		public string Href { set; get; }
		public string Src { set; get; }


		public BaseModel()
		{
			Body = string.Empty;
			Styles = string.Empty;
			Class = string.Empty;
			Href = string.Empty;
			Src = string.Empty;
		}
	}


}



