using System;
namespace Maticsoft.Model
{
	/// <summary>
	/// 实体类BlogCategory 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BlogCategory
	{
		public BlogCategory()
		{}
		#region Model
		private int _id;
		private int? _ownerid;
		private string _ownerurl;
		private int? _appid;
		private int? _orderid;
		private int? _parentid;
		private string _name;
		private string _description;
		private string _logo;
		private int? _datacount;
		private DateTime? _created;
		/// <summary>
		/// 
		/// </summary>
		public int Id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OwnerId
		{
			set{ _ownerid=value;}
			get{return _ownerid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OwnerUrl
		{
			set{ _ownerurl=value;}
			get{return _ownerurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AppId
		{
			set{ _appid=value;}
			get{return _appid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OrderId
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ParentId
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Logo
		{
			set{ _logo=value;}
			get{return _logo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? DataCount
		{
			set{ _datacount=value;}
			get{return _datacount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Created
		{
			set{ _created=value;}
			get{return _created;}
		}
		#endregion Model

	}
}

