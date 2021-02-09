using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace PaginationControl
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:PaginationToGV runat=server></{0}:PaginationToGV>")]
    public class PaginationToGV : CompositeControl
    {
        DropDownList drpPageIndex = new DropDownList();
        Label lblPageCount = new Label();
        Label lblPageSize = new Label();
        Label lblItemCount = new Label();
        LinkButton lBtnFirstPage;
        LinkButton lBtnPrePage;
        LinkButton lBtnNextPage;
        LinkButton lBtnLastPage;

        [Bindable(false)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(false)]

        public event EventHandler GvBind;
        decimal pagRows = Convert.ToDecimal(ConfigurationManager.AppSettings["ItemNumPerPage"]); 
        public string PageIndex
        {
            get
            {
                return drpPageIndex.SelectedValue;
            }
            set
            {
                drpPageIndex.SelectedValue = value;
            }
        }
        public string RowsCount
        {
            get
            {
                if (drpPageIndex.Items.Count < 1)
                {
                    return null;
                }
                else
                {
                    return drpPageIndex.Items[drpPageIndex.Items.Count - 1].Value;
                }
            }
            set
            {
                EnsureChildControls();
                drpPageIndex.Items.Clear();
                
                decimal cont = Convert.ToDecimal(value);
                decimal pages = decimal.Ceiling(cont / pagRows);
                for (int i = 1; i <= pages; i++)
                {
                    drpPageIndex.Items.Add(new ListItem(Convert.ToString(i), Convert.ToString(i)));
                }
            }
        }


        private void DDLBind()
        {
            EnsureChildControls();
            drpPageIndex.Items.Clear();
            decimal pagRows = Convert.ToDecimal(ConfigurationManager.AppSettings["ItemNumPerPage"]);
            decimal cont = Convert.ToDecimal(RowsCount);
            decimal pages = decimal.Ceiling(cont / pagRows);
            for (int i = 1; i <= pages; i++)
            {
                drpPageIndex.Items.Add(new ListItem(Convert.ToString(i), Convert.ToString(i)));
            }
            drpPageIndex.SelectedValue = PageIndex;
        }

        protected override bool OnBubbleEvent(object source, EventArgs e)
        {
            
            bool handled = false;

            if (e is CommandEventArgs)
            {
                CommandEventArgs ce = (CommandEventArgs)e;
                if (ce.CommandName == "FirstClick")
                {
                    OnFirstClick(ce);
                    handled = true;
                }
                else if (ce.CommandName == "PreClick")
                {
                    OnPreClick(ce);
                    handled = true;
                }
                else if (ce.CommandName == "NextClick")
                {
                    OnNextClick(ce);
                    handled = true;
                }
                else if (ce.CommandName == "LastClick")
                {
                    OnLastClick(ce);
                    handled = true;
                }

            }
            return handled;    
        }


        protected virtual void OnNextClick(EventArgs e)
        {
            if (GvBind != null)
            {
                int nowId = Convert.ToInt32(PageIndex)+1;
                PageIndex = Convert.ToString(nowId);
                GvBind(this, e);
            }
        }

        protected virtual void OnFirstClick(EventArgs e)
        {
            if (GvBind != null)
            {
                int nowId = 1;
                PageIndex = Convert.ToString(nowId);
                GvBind(this, e);
            }
        }

        protected virtual void OnPreClick(EventArgs e)
        {
            if (GvBind != null)
            {
                int nowId = Convert.ToInt32(PageIndex) - 1;
                if (nowId < 1)
                {
                    PageIndex = "1";
                }
                else
                {
                    PageIndex = Convert.ToString(nowId);
                }
                GvBind(this, e);
            }
        }

        protected virtual void OnLastClick(EventArgs e)
        {
            if (GvBind != null)
            {
                int nowId = Convert.ToInt32(drpPageIndex.Items[drpPageIndex.Items.Count-1].Value);
                PageIndex = Convert.ToString(nowId);
                GvBind(this, e);
            }
        }  


        protected override void CreateChildControls()
        {
            lBtnFirstPage = new LinkButton();
            lBtnFirstPage.Text = "首页";
            lBtnFirstPage.ID = "lBtnFirstPage";
            lBtnFirstPage.CommandName = "FirstClick";

            lBtnPrePage = new LinkButton();
            lBtnPrePage.Text = "上一页";
            lBtnPrePage.ID = "lBtnPrePage";
            lBtnPrePage.CommandName = "PreClick";
            

            lBtnNextPage = new LinkButton();
            lBtnNextPage.Text = "下一页";
            lBtnNextPage.ID = "lBtnNextPage";
            lBtnNextPage.CommandName = "NextClick";

            lBtnLastPage = new LinkButton();
            lBtnLastPage.Text = "末页";
            lBtnLastPage.ID = "lBtnLastPage";
            lBtnLastPage.CommandName = "LastClick";


            drpPageIndex.AutoPostBack = true;
            drpPageIndex.EnableViewState = true;
            drpPageIndex.SelectedIndexChanged += GvBind;

            this.Controls.Add(lBtnFirstPage);
            this.Controls.Add(lBtnPrePage);
            this.Controls.Add(lBtnNextPage);
            this.Controls.Add(lBtnLastPage);
            this.Controls.Add(drpPageIndex);
            
        }

        

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (PageIndex == RowsCount)
            {
                lBtnNextPage.Enabled = false;
                lBtnLastPage.Enabled = false;
            }
            if (PageIndex == "1")
            {
                lBtnPrePage.Enabled = false;
                lBtnFirstPage.Enabled = false;
            }
            lBtnFirstPage.RenderControl(output);
            lBtnPrePage.RenderControl(output);
            lBtnNextPage.RenderControl(output);
            lBtnLastPage.RenderControl(output);
            drpPageIndex.RenderControl(output);
            output.Write("第" + PageIndex + "页&nbsp;" + Convert.ToString(pagRows) + "条/页&nbsp;共" + RowsCount + "页");
        }
    }
}
