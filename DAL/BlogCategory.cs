using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//请先添加引用
namespace Maticsoft.DAL
{
	/// <summary>
	/// 数据访问类BlogCategory。
	/// </summary>
	public class BlogCategory
	{
		public BlogCategory()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Id", "BlogCategory"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from BlogCategory");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = Id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Maticsoft.Model.BlogCategory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BlogCategory(");
			strSql.Append("OwnerId,OwnerUrl,AppId,OrderId,ParentId,Name,Description,Logo,DataCount,Created)");
			strSql.Append(" values (");
			strSql.Append("@OwnerId,@OwnerUrl,@AppId,@OrderId,@ParentId,@Name,@Description,@Logo,@DataCount,@Created)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@OwnerId", SqlDbType.Int,4),
					new SqlParameter("@OwnerUrl", SqlDbType.NVarChar,150),
					new SqlParameter("@AppId", SqlDbType.Int,4),
					new SqlParameter("@OrderId", SqlDbType.Int,4),
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Description", SqlDbType.NText),
					new SqlParameter("@Logo", SqlDbType.NVarChar,150),
					new SqlParameter("@DataCount", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime)};
			parameters[0].Value = model.OwnerId;
			parameters[1].Value = model.OwnerUrl;
			parameters[2].Value = model.AppId;
			parameters[3].Value = model.OrderId;
			parameters[4].Value = model.ParentId;
			parameters[5].Value = model.Name;
			parameters[6].Value = model.Description;
			parameters[7].Value = model.Logo;
			parameters[8].Value = model.DataCount;
			parameters[9].Value = model.Created;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 1;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(Maticsoft.Model.BlogCategory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BlogCategory set ");
			strSql.Append("OwnerId=@OwnerId,");
			strSql.Append("OwnerUrl=@OwnerUrl,");
			strSql.Append("AppId=@AppId,");
			strSql.Append("OrderId=@OrderId,");
			strSql.Append("ParentId=@ParentId,");
			strSql.Append("Name=@Name,");
			strSql.Append("Description=@Description,");
			strSql.Append("Logo=@Logo,");
			strSql.Append("DataCount=@DataCount,");
			strSql.Append("Created=@Created");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4),
					new SqlParameter("@OwnerId", SqlDbType.Int,4),
					new SqlParameter("@OwnerUrl", SqlDbType.NVarChar,150),
					new SqlParameter("@AppId", SqlDbType.Int,4),
					new SqlParameter("@OrderId", SqlDbType.Int,4),
					new SqlParameter("@ParentId", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@Description", SqlDbType.NText),
					new SqlParameter("@Logo", SqlDbType.NVarChar,150),
					new SqlParameter("@DataCount", SqlDbType.Int,4),
					new SqlParameter("@Created", SqlDbType.DateTime)};
			parameters[0].Value = model.Id;
			parameters[1].Value = model.OwnerId;
			parameters[2].Value = model.OwnerUrl;
			parameters[3].Value = model.AppId;
			parameters[4].Value = model.OrderId;
			parameters[5].Value = model.ParentId;
			parameters[6].Value = model.Name;
			parameters[7].Value = model.Description;
			parameters[8].Value = model.Logo;
			parameters[9].Value = model.DataCount;
			parameters[10].Value = model.Created;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BlogCategory ");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = Id;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Maticsoft.Model.BlogCategory GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,OwnerId,OwnerUrl,AppId,OrderId,ParentId,Name,Description,Logo,DataCount,Created from BlogCategory ");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = Id;

			Maticsoft.Model.BlogCategory model=new Maticsoft.Model.BlogCategory();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Id"].ToString()!="")
				{
					model.Id=int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["OwnerId"].ToString()!="")
				{
					model.OwnerId=int.Parse(ds.Tables[0].Rows[0]["OwnerId"].ToString());
				}
				model.OwnerUrl=ds.Tables[0].Rows[0]["OwnerUrl"].ToString();
				if(ds.Tables[0].Rows[0]["AppId"].ToString()!="")
				{
					model.AppId=int.Parse(ds.Tables[0].Rows[0]["AppId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["OrderId"].ToString()!="")
				{
					model.OrderId=int.Parse(ds.Tables[0].Rows[0]["OrderId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["ParentId"].ToString()!="")
				{
					model.ParentId=int.Parse(ds.Tables[0].Rows[0]["ParentId"].ToString());
				}
				model.Name=ds.Tables[0].Rows[0]["Name"].ToString();
				model.Description=ds.Tables[0].Rows[0]["Description"].ToString();
				model.Logo=ds.Tables[0].Rows[0]["Logo"].ToString();
				if(ds.Tables[0].Rows[0]["DataCount"].ToString()!="")
				{
					model.DataCount=int.Parse(ds.Tables[0].Rows[0]["DataCount"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Created"].ToString()!="")
				{
					model.Created=DateTime.Parse(ds.Tables[0].Rows[0]["Created"].ToString());
				}
				return model;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Id,OwnerId,OwnerUrl,AppId,OrderId,ParentId,Name,Description,Logo,DataCount,Created ");
			strSql.Append(" FROM BlogCategory ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" Id,OwnerId,OwnerUrl,AppId,OrderId,ParentId,Name,Description,Logo,DataCount,Created ");
			strSql.Append(" FROM BlogCategory ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "BlogCategory";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  成员方法
	}
}

