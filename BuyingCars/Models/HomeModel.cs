using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyingMetal.Models
{

	public class HomeModel
	{
		public List<LinkModel> HeadMenuLeft { set; get; }
		public List<LinkModel> HeadMenuRight { set; get; }
		public HeaderModel Header { set; get; }

		public HomeModel()
		{
			this.HeadMenuLeft = new List<LinkModel>();
			this.HeadMenuRight = new List<LinkModel>();
			this.Header = new HeaderModel();
		}
	}

	public class LinkModel
	{
		public int Id { set; get; }
		public string Text { set; get; }
		public string Href { set; get; }
		public string Classes { set; get; }
		public string Target { set; get; }

		public LinkModel()
		{
			this.Text = this.Href = this.Classes = this.Target = string.Empty;
		}
	}


	public class HeaderModel
	{
		public int Id { set; get; }
		public string Text1 { set; get; }
		public string Text2 { set; get; }
		public string Text3 { set; get; }
		public LinkModel Link1 { set; get; }
		public LinkModel Link2 { set; get; }
		public LinkModel Link3 { set; get; }

		public HeaderModel()
		{
			this.Link1 = new LinkModel();
			this.Link2 = new LinkModel();
			this.Link3 = new LinkModel();
			this.Text1 = this.Text2 = this.Text3 = string.Empty;
		}
	}


}



