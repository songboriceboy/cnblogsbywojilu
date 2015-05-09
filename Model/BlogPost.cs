using System;
namespace Maticsoft.Model
{
	/// <summary>
	/// 实体类BlogPost 。(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public class BlogPost
	{
		public BlogPost()
		{}
		#region Model
		private int _id;
		private int? _ownerid;
		private string _ownertype;
		private string _ownerurl;
		private int? _appid;
		private int? _syscategoryid;
		private int? _creatorid;
		private string _creatorurl;
		private int? _categoryid;
		private string _title;
		private string _content;
		private string _abstract;
		private string _pic;
		private int? _commentcondition;
		private int? _hits;
		private int? _ispic;
		private int? _ispick;
		private int? _istop;
		private int? _replies;
		private int? _savestatus;
		private int? _accessstatus;
		private string _ip;
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
		public string OwnerType
		{
			set{ _ownertype=value;}
			get{return _ownertype;}
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
		public int? SysCategoryId
		{
			set{ _syscategoryid=value;}
			get{return _syscategoryid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CreatorId
		{
			set{ _creatorid=value;}
			get{return _creatorid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CreatorUrl
		{
			set{ _creatorurl=value;}
			get{return _creatorurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CategoryId
		{
			set{ _categoryid=value;}
			get{return _categoryid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Content
		{
			set{ _content=value;}
			get{return _content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Abstract
		{
			set{ _abstract=value;}
			get{return _abstract;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Pic
		{
			set{ _pic=value;}
			get{return _pic;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? CommentCondition
		{
			set{ _commentcondition=value;}
			get{return _commentcondition;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Hits
		{
			set{ _hits=value;}
			get{return _hits;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsPic
		{
			set{ _ispic=value;}
			get{return _ispic;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsPick
		{
			set{ _ispick=value;}
			get{return _ispick;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? IsTop
		{
			set{ _istop=value;}
			get{return _istop;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Replies
		{
			set{ _replies=value;}
			get{return _replies;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? SaveStatus
		{
			set{ _savestatus=value;}
			get{return _savestatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? AccessStatus
		{
			set{ _accessstatus=value;}
			get{return _accessstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Ip
		{
			set{ _ip=value;}
			get{return _ip;}
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

