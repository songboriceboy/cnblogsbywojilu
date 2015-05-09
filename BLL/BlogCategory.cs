using System;
using System.Data;
using System.Collections.Generic;
//using LTP.Common;
using Maticsoft.Model;
namespace Maticsoft.BLL
{
	/// <summary>
	/// 业务逻辑类BlogCategory 的摘要说明。
	/// </summary>
	public class BlogCategory
	{
		private readonly Maticsoft.DAL.BlogCategory dal=new Maticsoft.DAL.BlogCategory();
		public BlogCategory()
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
		public int  Add(Maticsoft.Model.BlogCategory model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(Maticsoft.Model.BlogCategory model)
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
		public Maticsoft.Model.BlogCategory GetModel(int Id)
		{
			
			return dal.GetModel(Id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
        //public Maticsoft.Model.BlogCategory GetModelByCache(int Id)
        //{
			
        //    string CacheKey = "BlogCategoryModel-" + Id;
        //    object objModel = LTP.Common.DataCache.GetCache(CacheKey);
        //    if (objModel == null)
        //    {
        //        try
        //        {
        //            objModel = dal.GetModel(Id);
        //            if (objModel != null)
        //            {
        //                int ModelCache = LTP.Common.ConfigHelper.GetConfigInt("ModelCache");
        //                LTP.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
        //            }
        //        }
        //        catch{}
        //    }
        //    return (Maticsoft.Model.BlogCategory)objModel;
        //}

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
		public List<Maticsoft.Model.BlogCategory> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Maticsoft.Model.BlogCategory> DataTableToList(DataTable dt)
		{
			List<Maticsoft.Model.BlogCategory> modelList = new List<Maticsoft.Model.BlogCategory>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Maticsoft.Model.BlogCategory model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new Maticsoft.Model.BlogCategory();
					if(dt.Rows[n]["Id"].ToString()!="")
					{
						model.Id=int.Parse(dt.Rows[n]["Id"].ToString());
					}
					if(dt.Rows[n]["OwnerId"].ToString()!="")
					{
						model.OwnerId=int.Parse(dt.Rows[n]["OwnerId"].ToString());
					}
					model.OwnerUrl=dt.Rows[n]["OwnerUrl"].ToString();
					if(dt.Rows[n]["AppId"].ToString()!="")
					{
						model.AppId=int.Parse(dt.Rows[n]["AppId"].ToString());
					}
					if(dt.Rows[n]["OrderId"].ToString()!="")
					{
						model.OrderId=int.Parse(dt.Rows[n]["OrderId"].ToString());
					}
					if(dt.Rows[n]["ParentId"].ToString()!="")
					{
						model.ParentId=int.Parse(dt.Rows[n]["ParentId"].ToString());
					}
					model.Name=dt.Rows[n]["Name"].ToString();
					model.Description=dt.Rows[n]["Description"].ToString();
					model.Logo=dt.Rows[n]["Logo"].ToString();
					if(dt.Rows[n]["DataCount"].ToString()!="")
					{
						model.DataCount=int.Parse(dt.Rows[n]["DataCount"].ToString());
					}
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

