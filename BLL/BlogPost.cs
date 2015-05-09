using System;
using System.Data;
using System.Collections.Generic;
//using LTP.Common;
using Maticsoft.Model;
namespace Maticsoft.BLL
{
	/// <summary>
	/// 业务逻辑类BlogPost 的摘要说明。
	/// </summary>
	public class BlogPost
	{
		private readonly Maticsoft.DAL.BlogPost dal=new Maticsoft.DAL.BlogPost();
		public BlogPost()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			return dal.Exists(Id);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(Maticsoft.Model.BlogPost model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(Maticsoft.Model.BlogPost model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int Id)
		{
			
			dal.Delete(Id);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.BlogPost GetModel(int Id)
		{
			
			return dal.GetModel(Id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Maticsoft.Model.BlogPost> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Maticsoft.Model.BlogPost> DataTableToList(DataTable dt)
		{
			List<Maticsoft.Model.BlogPost> modelList = new List<Maticsoft.Model.BlogPost>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Maticsoft.Model.BlogPost model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new Maticsoft.Model.BlogPost();
					if(dt.Rows[n]["Id"].ToString()!="")
					{
						model.Id=int.Parse(dt.Rows[n]["Id"].ToString());
					}
					if(dt.Rows[n]["OwnerId"].ToString()!="")
					{
						model.OwnerId=int.Parse(dt.Rows[n]["OwnerId"].ToString());
					}
					model.OwnerType=dt.Rows[n]["OwnerType"].ToString();
					model.OwnerUrl=dt.Rows[n]["OwnerUrl"].ToString();
					if(dt.Rows[n]["AppId"].ToString()!="")
					{
						model.AppId=int.Parse(dt.Rows[n]["AppId"].ToString());
					}
					if(dt.Rows[n]["SysCategoryId"].ToString()!="")
					{
						model.SysCategoryId=int.Parse(dt.Rows[n]["SysCategoryId"].ToString());
					}
					if(dt.Rows[n]["CreatorId"].ToString()!="")
					{
						model.CreatorId=int.Parse(dt.Rows[n]["CreatorId"].ToString());
					}
					model.CreatorUrl=dt.Rows[n]["CreatorUrl"].ToString();
					if(dt.Rows[n]["CategoryId"].ToString()!="")
					{
						model.CategoryId=int.Parse(dt.Rows[n]["CategoryId"].ToString());
					}
					model.Title=dt.Rows[n]["Title"].ToString();
					model.Content=dt.Rows[n]["Content"].ToString();
					model.Abstract=dt.Rows[n]["Abstract"].ToString();
					model.Pic=dt.Rows[n]["Pic"].ToString();
					if(dt.Rows[n]["CommentCondition"].ToString()!="")
					{
						model.CommentCondition=int.Parse(dt.Rows[n]["CommentCondition"].ToString());
					}
					if(dt.Rows[n]["Hits"].ToString()!="")
					{
						model.Hits=int.Parse(dt.Rows[n]["Hits"].ToString());
					}
					if(dt.Rows[n]["IsPic"].ToString()!="")
					{
						model.IsPic=int.Parse(dt.Rows[n]["IsPic"].ToString());
					}
					if(dt.Rows[n]["IsPick"].ToString()!="")
					{
						model.IsPick=int.Parse(dt.Rows[n]["IsPick"].ToString());
					}
					if(dt.Rows[n]["IsTop"].ToString()!="")
					{
						model.IsTop=int.Parse(dt.Rows[n]["IsTop"].ToString());
					}
					if(dt.Rows[n]["Replies"].ToString()!="")
					{
						model.Replies=int.Parse(dt.Rows[n]["Replies"].ToString());
					}
					if(dt.Rows[n]["SaveStatus"].ToString()!="")
					{
						model.SaveStatus=int.Parse(dt.Rows[n]["SaveStatus"].ToString());
					}
					if(dt.Rows[n]["AccessStatus"].ToString()!="")
					{
						model.AccessStatus=int.Parse(dt.Rows[n]["AccessStatus"].ToString());
					}
					model.Ip=dt.Rows[n]["Ip"].ToString();
					if(dt.Rows[n]["Created"].ToString()!="")
					{
						model.Created=DateTime.Parse(dt.Rows[n]["Created"].ToString());
					}
					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  成员方法
	}
}

