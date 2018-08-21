namespace LambdaAndGeneric.Common
{
    using NPOI.HPSF;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using System;
    using System.Collections;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;

    /// <summary>
    /// 导出 Excel 
    /// </summary>
    public static class NPOIHelper
    {
        public static MemoryStream DaoChuLieBiao(DataTable dtData, string strHeaderText, DataTable dtHeader)
        {
            int num2;
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
            DocumentSummaryInformation information = PropertySetFactory.CreateDocumentSummaryInformation();
            information.Company = "管理系统";
            workbook.DocumentSummaryInformation = information;
            SummaryInformation information2 = PropertySetFactory.CreateSummaryInformation();
            information2.Author = "Admin";
            information2.ApplicationName = "管理系统";
            information2.LastAuthor = "admin";
            information2.Comments = "管理系统";
            information2.Title = "管理系统";
            information2.Subject = "管理系统";
            information2.CreateDateTime = new DateTime?(DateTime.Now);
            workbook.SummaryInformation = information2;
            HSSFCellStyle style = workbook.CreateCellStyle() as HSSFCellStyle;
            style.DataFormat = (workbook.CreateDataFormat() as HSSFDataFormat).GetFormat("yyyy-mm-dd");
            style.Alignment = HorizontalAlignment.CENTER;
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            int[] numArray = new int[dtData.Columns.Count];
            foreach (DataColumn column in dtData.Columns)
            {
                numArray[column.Ordinal] = Encoding.GetEncoding(0x3a8).GetBytes(column.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                num2 = 0;
                while (num2 < dtData.Columns.Count)
                {
                    int length = Encoding.GetEncoding(0x3a8).GetBytes(dtData.Rows[i][num2].ToString()).Length;
                    if (length > numArray[num2])
                    {
                        numArray[num2] = length;
                    }
                    num2++;
                }
            }
            int rownum = 0;
            foreach (DataRow row in dtData.Rows)
            {
                switch (rownum)
                {
                    case 0xffff:
                    case 0:
                        {
                            if (rownum != 0)
                            {
                                sheet = workbook.CreateSheet() as HSSFSheet;
                            }
                            HSSFRow row2 = sheet.CreateRow(0) as HSSFRow;
                            row2.HeightInPoints = 25f;
                            row2.CreateCell(0).SetCellValue(strHeaderText);
                            HSSFCellStyle style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                            style2.Alignment = HorizontalAlignment.CENTER;
                            style2.BorderBottom = BorderStyle.THIN;
                            style2.BorderLeft = BorderStyle.THIN;
                            style2.BorderRight = BorderStyle.THIN;
                            style2.BorderTop = BorderStyle.THIN;
                            HSSFFont font = workbook.CreateFont() as HSSFFont;
                            font.FontHeightInPoints = 20;
                            font.Boldweight = 700;
                            font.FontName = "宋体";
                            style2.SetFont(font);
                            row2.GetCell(0).CellStyle = style2;
                            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtData.Columns.Count - 1));
                            row2 = sheet.CreateRow(1) as HSSFRow;
                            HSSFRow row3 = sheet.CreateRow(2) as HSSFRow;
                            style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                            style2.Alignment = HorizontalAlignment.CENTER;
                            style2.BorderBottom = BorderStyle.THIN;
                            style2.BorderLeft = BorderStyle.THIN;
                            style2.BorderRight = BorderStyle.THIN;
                            style2.BorderTop = BorderStyle.THIN;
                            font = workbook.CreateFont() as HSSFFont;
                            font.FontName = "宋体";
                            style2.IsLocked = true;
                            style2.SetFont(font);
                            int num5 = 0;
                            for (num2 = 0; num2 < dtHeader.Columns.Count; num2++)
                            {
                                row2.CreateCell(num5).SetCellValue(dtHeader.Columns[num2].ColumnName.ToString());
                                row2.GetCell(num5).CellStyle = style2;
                                num5++;
                            }
                            rownum = 2;
                            break;
                        }
                }
                HSSFRow row4 = sheet.CreateRow(rownum) as HSSFRow;
                HSSFCellStyle style3 = workbook.CreateCellStyle() as HSSFCellStyle;
                style3.Alignment = HorizontalAlignment.CENTER;
                style3.BorderBottom = BorderStyle.THIN;
                style3.BorderLeft = BorderStyle.THIN;
                style3.BorderRight = BorderStyle.THIN;
                style3.BorderTop = BorderStyle.THIN;
                HSSFFont font2 = workbook.CreateFont() as HSSFFont;
                font2.Boldweight = 400;
                font2.FontName = "宋体";
                style3.SetFont(font2);
                foreach (DataColumn column2 in dtData.Columns)
                {
                    HSSFCell cell = row4.CreateCell(column2.Ordinal) as HSSFCell;
                    string str = row[column2].ToString();
                    switch (column2.DataType.ToString())
                    {
                        case "System.String":
                            cell.SetCellValue(str);
                            cell.CellStyle = style3;
                            goto Label_06EE;

                        case "System.DateTime":
                            DateTime time;
                            if (!DateTime.TryParse(str, out time))
                            {
                                break;
                            }
                            cell.SetCellValue(time);
                            goto Label_0613;

                        case "System.Boolean":
                            {
                                bool result = false;
                                bool.TryParse(str, out result);
                                cell.SetCellValue(result);
                                cell.CellStyle = style3;
                                goto Label_06EE;
                            }
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            {
                                int num6 = 0;
                                if (!int.TryParse(str, out num6))
                                {
                                    goto Label_066C;
                                }
                                cell.SetCellValue((double)num6);
                                goto Label_067B;
                            }
                        case "System.Decimal":
                            goto Label_06EE;

                        case "System.Double":
                            {
                                double num8 = 0.0;
                                double.TryParse(str, out num8);
                                cell.SetCellValue(num8);
                                cell.CellStyle = style3;
                                goto Label_06EE;
                            }
                        case "System.DBNull":
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            goto Label_06EE;

                        default:
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            goto Label_06EE;
                    }
                    cell.SetCellValue("");
                    Label_0613:
                    cell.CellStyle = style;
                    goto Label_06EE;
                    Label_066C:
                    cell.SetCellValue("");
                    Label_067B:
                    cell.CellStyle = style3;
                    Label_06EE:;
                }
                rownum++;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);
                stream.Flush();
                stream.Position = 0L;
                return stream;
            }
        }

        public static MemoryStream Export(DataTable dtSource, string strHeaderText, Hashtable nameList)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
            DocumentSummaryInformation information = PropertySetFactory.CreateDocumentSummaryInformation();
            information.Company = "CRM客户管理系统";
            workbook.DocumentSummaryInformation = information;
            SummaryInformation information2 = PropertySetFactory.CreateSummaryInformation();
            information2.Author = "Admin";
            information2.ApplicationName = "CRM客户管理系统";
            information2.LastAuthor = "admin";
            information2.Comments = "CRM客户管理系统";
            information2.Title = "CRM客户管理系统";
            information2.Subject = "CRM客户管理系统";
            information2.CreateDateTime = new DateTime?(DateTime.Now);
            workbook.SummaryInformation = information2;
            HSSFCellStyle style = workbook.CreateCellStyle() as HSSFCellStyle;
            style.DataFormat = (workbook.CreateDataFormat() as HSSFDataFormat).GetFormat("yyyy-mm-dd");
            int[] numArray = new int[dtSource.Columns.Count];
            foreach (DataColumn column in dtSource.Columns)
            {
                numArray[column.Ordinal] = Encoding.GetEncoding(0x3a8).GetBytes(column.ColumnName.ToString()).Length;
            }
            int num = 0;
            while (num < dtSource.Rows.Count)
            {
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    int length = Encoding.GetEncoding(0x3a8).GetBytes(dtSource.Rows[num][i].ToString()).Length;
                    if (length > numArray[i])
                    {
                        numArray[i] = length;
                    }
                }
                num++;
            }
            int rownum = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                HSSFFont font;
                switch (rownum)
                {
                    case 0xffff:
                    case 0:
                        {
                            if (rownum != 0)
                            {
                                sheet = workbook.CreateSheet() as HSSFSheet;
                            }
                            HSSFRow row2 = sheet.CreateRow(0) as HSSFRow;
                            row2.HeightInPoints = 25f;
                            row2.CreateCell(0).SetCellValue(strHeaderText);
                            HSSFCellStyle style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                            style2.Alignment = HorizontalAlignment.CENTER;
                            style2.BorderBottom = BorderStyle.THIN;
                            style2.BorderLeft = BorderStyle.THIN;
                            style2.BorderRight = BorderStyle.THIN;
                            style2.BorderTop = BorderStyle.THIN;
                            font = workbook.CreateFont() as HSSFFont;
                            font.FontHeightInPoints = 20;
                            font.Boldweight = 700;
                            font.FontName = "宋体";
                            style2.SetFont(font);
                            row2.GetCell(0).CellStyle = style2;
                            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                            row2 = sheet.CreateRow(1) as HSSFRow;
                            style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                            style2.Alignment = HorizontalAlignment.CENTER;
                            style2.BorderBottom = BorderStyle.THIN;
                            style2.BorderLeft = BorderStyle.THIN;
                            style2.BorderRight = BorderStyle.THIN;
                            style2.BorderTop = BorderStyle.THIN;
                            font = workbook.CreateFont() as HSSFFont;
                            font.FontName = "宋体";
                            style2.IsLocked = true;
                            style2.SetFont(font);
                            num = 0;
                            foreach (DataColumn column2 in dtSource.Columns)
                            {
                                string str = column2.ColumnName.Trim();
                                object obj2 = str;
                                IDictionaryEnumerator enumerator = nameList.GetEnumerator();
                                while (enumerator.MoveNext())
                                {
                                    if (enumerator.Key.ToString().Trim() == str)
                                    {
                                        obj2 = enumerator.Value;
                                    }
                                }
                                row2.CreateCell(num).SetCellValue(obj2.ToString());
                                row2.GetCell(num).CellStyle = style2;
                                sheet.SetColumnWidth(num, (numArray[num] + 1) * 0x100);
                                num++;
                            }
                            sheet.CreateFreezePane(0, 2, 0, dtSource.Columns.Count - 1);
                            rownum = 2;
                            break;
                        }
                }
                HSSFRow row3 = sheet.CreateRow(rownum) as HSSFRow;
                foreach (DataColumn column3 in dtSource.Columns)
                {
                    HSSFCell cell = row3.CreateCell(column3.Ordinal) as HSSFCell;
                    HSSFCellStyle style3 = workbook.CreateCellStyle() as HSSFCellStyle;
                    style3.Alignment = HorizontalAlignment.CENTER;
                    style3.BorderBottom = BorderStyle.THIN;
                    style3.BorderLeft = BorderStyle.THIN;
                    style3.BorderRight = BorderStyle.THIN;
                    style3.BorderTop = BorderStyle.THIN;
                    font = workbook.CreateFont() as HSSFFont;
                    font.Boldweight = 400;
                    font.FontName = "宋体";
                    style3.SetFont(font);
                    string str2 = row[column3].ToString();
                    switch (column3.DataType.ToString())
                    {
                        case "System.String":
                            cell.SetCellValue(str2);
                            cell.CellStyle = style3;
                            break;

                        case "System.DateTime":
                            DateTime time;
                            DateTime.TryParse(str2, out time);
                            cell.SetCellValue(time);
                            cell.CellStyle = style;
                            break;

                        case "System.Boolean":
                            {
                                bool result = false;
                                bool.TryParse(str2, out result);
                                cell.SetCellValue(result);
                                cell.CellStyle = style3;
                                break;
                            }
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            {
                                int num5 = 0;
                                int.TryParse(str2, out num5);
                                cell.SetCellValue((double)num5);
                                cell.CellStyle = style3;
                                break;
                            }
                        case "System.Decimal":
                        case "System.Double":
                            {
                                double num6 = 0.0;
                                double.TryParse(str2, out num6);
                                cell.SetCellValue(num6);
                                cell.CellStyle = style3;
                                break;
                            }
                        case "System.DBNull":
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            break;

                        default:
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            break;
                    }
                }
                rownum++;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);
                stream.Flush();
                stream.Position = 0L;
                return stream;
            }
        }

        public static MemoryStream exportAcount(DataTable dtData, string strHeaderText, DataTable dtHeader)
        {
            HSSFRow row2;
            HSSFCellStyle style2;
            HSSFFont font;
            HSSFRow row3;
            int num3;
            int num4;
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
            DocumentSummaryInformation information = PropertySetFactory.CreateDocumentSummaryInformation();
            information.Company = "管理系统";
            workbook.DocumentSummaryInformation = information;
            SummaryInformation information2 = PropertySetFactory.CreateSummaryInformation();
            information2.Author = "Admin";
            information2.ApplicationName = "管理系统";
            information2.LastAuthor = "admin";
            information2.Comments = "管理系统";
            information2.Title = "管理系统";
            information2.Subject = "管理系统";
            information2.CreateDateTime = new DateTime?(DateTime.Now);
            workbook.SummaryInformation = information2;
            HSSFCellStyle style = workbook.CreateCellStyle() as HSSFCellStyle;
            style.DataFormat = (workbook.CreateDataFormat() as HSSFDataFormat).GetFormat("yyyy-mm-dd");
            style.Alignment = HorizontalAlignment.CENTER;
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            int rownum = 0;
            int num2 = 0;
            foreach (DataRow row in dtData.Rows)
            {
                if (num2 == (dtData.Rows.Count - 1))
                {
                    break;
                }
                num2++;
                switch (rownum)
                {
                    case 0xffff:
                    case 0:
                        if (rownum != 0)
                        {
                            sheet = workbook.CreateSheet() as HSSFSheet;
                        }
                        row2 = sheet.CreateRow(0) as HSSFRow;
                        row2.HeightInPoints = 25f;
                        row2.CreateCell(0).SetCellValue(strHeaderText);
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        font.FontName = "宋体";
                        style2.SetFont(font);
                        row2.GetCell(0).CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtData.Columns.Count - 1));
                        row2 = sheet.CreateRow(1) as HSSFRow;
                        row3 = sheet.CreateRow(2) as HSSFRow;
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontName = "宋体";
                        style2.IsLocked = true;
                        style2.SetFont(font);
                        num3 = 0;
                        num4 = 0;
                        while (num4 < dtHeader.Columns.Count)
                        {
                            row2.CreateCell(num3).SetCellValue(dtHeader.Columns[num4].ColumnName.ToString());
                            row2.GetCell(num3).CellStyle = style2;
                            num3++;
                            num4++;
                        }
                        rownum = 2;
                        break;
                }
                HSSFRow row4 = sheet.CreateRow(rownum) as HSSFRow;
                HSSFCellStyle style3 = workbook.CreateCellStyle() as HSSFCellStyle;
                style3.Alignment = HorizontalAlignment.CENTER;
                style3.BorderBottom = BorderStyle.THIN;
                style3.BorderLeft = BorderStyle.THIN;
                style3.BorderRight = BorderStyle.THIN;
                style3.BorderTop = BorderStyle.THIN;
                HSSFFont font2 = workbook.CreateFont() as HSSFFont;
                font2.Boldweight = 400;
                font2.FontName = "宋体";
                style3.SetFont(font2);
                int num5 = 0;
                foreach (DataColumn column in dtData.Columns)
                {
                    HSSFCell cell = row4.CreateCell(column.Ordinal) as HSSFCell;
                    string str = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String":
                            cell.SetCellValue(str);
                            cell.CellStyle = style3;
                            goto Label_05FC;

                        case "System.DateTime":
                            DateTime time;
                            if (!DateTime.TryParse(str, out time))
                            {
                                break;
                            }
                            cell.SetCellValue(time);
                            goto Label_052B;

                        case "System.Boolean":
                            {
                                bool result = false;
                                bool.TryParse(str, out result);
                                cell.SetCellValue(result);
                                cell.CellStyle = style3;
                                goto Label_05FC;
                            }
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            {
                                int num6 = 0;
                                if (!int.TryParse(str, out num6))
                                {
                                    goto Label_0584;
                                }
                                cell.SetCellValue((double)num6);
                                goto Label_0593;
                            }
                        case "System.Decimal":
                        case "System.Double":
                            {
                                double num7 = 0.0;
                                double.TryParse(str, out num7);
                                cell.SetCellValue(num7);
                                cell.CellStyle = style3;
                                goto Label_05FC;
                            }
                        case "System.DBNull":
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            goto Label_05FC;

                        default:
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            goto Label_05FC;
                    }
                    cell.SetCellValue("");
                    Label_052B:
                    cell.CellStyle = style;
                    goto Label_05FC;
                    Label_0584:
                    cell.SetCellValue("");
                    Label_0593:
                    cell.CellStyle = style3;
                    Label_05FC:;
                }
                num5++;
                rownum++;
            }
            if ((num2 + 1) == dtData.Rows.Count)
            {
                switch (rownum)
                {
                    case 0xffff:
                    case 0:
                        if (rownum != 0)
                        {
                            sheet = workbook.CreateSheet() as HSSFSheet;
                        }
                        row2 = sheet.CreateRow(0) as HSSFRow;
                        row2.HeightInPoints = 25f;
                        row2.CreateCell(0).SetCellValue(strHeaderText);
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        font.FontName = "宋体";
                        style2.SetFont(font);
                        row2.GetCell(0).CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtData.Columns.Count - 1));
                        row2 = sheet.CreateRow(1) as HSSFRow;
                        row3 = sheet.CreateRow(2) as HSSFRow;
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontName = "宋体";
                        style2.IsLocked = true;
                        style2.SetFont(font);
                        num3 = 0;
                        for (num4 = 0; num4 < dtHeader.Columns.Count; num4++)
                        {
                            row2.CreateCell(num3).SetCellValue(dtHeader.Columns[num4].ColumnName.ToString());
                            row2.GetCell(num3).CellStyle = style2;
                            num3++;
                        }
                        rownum = 2;
                        break;
                }
                HSSFCellStyle style4 = workbook.CreateCellStyle() as HSSFCellStyle;
                style4.Alignment = HorizontalAlignment.RIGHT;
                HSSFFont font3 = workbook.CreateFont() as HSSFFont;
                font3.Boldweight = 700;
                font3.FontName = "宋体";
                style4.SetFont(font3);
                HSSFRow row5 = sheet.CreateRow(rownum) as HSSFRow;
                CellRangeAddress region = new CellRangeAddress(rownum, rownum, 0, dtData.Columns.Count - 1);
                sheet.AddMergedRegion(region);
                row5.CreateCell(0).SetCellValue("合计 " + dtData.Rows[num2]["inOut"].ToString() + ": " + dtData.Rows[num2]["money"].ToString() + "   支出: " + dtData.Rows[num2]["createMan"].ToString());
                row5.GetCell(0).CellStyle = style4;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);
                stream.Flush();
                stream.Position = 0L;
                return stream;
            }
        }

        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName, Hashtable nameList)
        {
            HttpContext current = HttpContext.Current;
            current.Response.ContentType = "application/vnd.ms-excel";
            current.Response.ContentEncoding = Encoding.UTF8;
            current.Response.Charset = "";
            current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));
            current.Response.BinaryWrite(Export(dtSource, strHeaderText, nameList).GetBuffer());
            current.Response.End();
        }

        public static MemoryStream exportHT(DataTable dtData, string strHeaderText, DataTable dtHeader, decimal all, decimal have, decimal no)
        {
            HSSFRow row2;
            HSSFCellStyle style2;
            HSSFFont font;
            HSSFRow row3;
            int num2;
            int num3;
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
            DocumentSummaryInformation information = PropertySetFactory.CreateDocumentSummaryInformation();
            information.Company = "管理系统";
            workbook.DocumentSummaryInformation = information;
            SummaryInformation information2 = PropertySetFactory.CreateSummaryInformation();
            information2.Author = "Admin";
            information2.ApplicationName = "管理系统";
            information2.LastAuthor = "admin";
            information2.Comments = "管理系统";
            information2.Title = "管理系统";
            information2.Subject = "管理系统";
            information2.CreateDateTime = new DateTime?(DateTime.Now);
            workbook.SummaryInformation = information2;
            HSSFCellStyle style = workbook.CreateCellStyle() as HSSFCellStyle;
            style.DataFormat = (workbook.CreateDataFormat() as HSSFDataFormat).GetFormat("yyyy-mm-dd");
            style.Alignment = HorizontalAlignment.CENTER;
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            int rownum = 0;
            foreach (DataRow row in dtData.Rows)
            {
                switch (rownum)
                {
                    case 0xffff:
                    case 0:
                        if (rownum != 0)
                        {
                            sheet = workbook.CreateSheet() as HSSFSheet;
                        }
                        row2 = sheet.CreateRow(0) as HSSFRow;
                        row2.HeightInPoints = 25f;
                        row2.CreateCell(0).SetCellValue(strHeaderText);
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        font.FontName = "宋体";
                        style2.SetFont(font);
                        row2.GetCell(0).CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtData.Columns.Count - 1));
                        row2 = sheet.CreateRow(1) as HSSFRow;
                        row3 = sheet.CreateRow(2) as HSSFRow;
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontName = "宋体";
                        style2.IsLocked = true;
                        style2.SetFont(font);
                        num2 = 0;
                        num3 = 0;
                        while (num3 < dtHeader.Columns.Count)
                        {
                            row2.CreateCell(num2).SetCellValue(dtHeader.Columns[num3].ColumnName.ToString());
                            row2.GetCell(num2).CellStyle = style2;
                            num2++;
                            num3++;
                        }
                        rownum = 2;
                        break;
                }
                HSSFRow row4 = sheet.CreateRow(rownum) as HSSFRow;
                HSSFCellStyle style3 = workbook.CreateCellStyle() as HSSFCellStyle;
                style3.Alignment = HorizontalAlignment.CENTER;
                style3.BorderBottom = BorderStyle.THIN;
                style3.BorderLeft = BorderStyle.THIN;
                style3.BorderRight = BorderStyle.THIN;
                style3.BorderTop = BorderStyle.THIN;
                HSSFFont font2 = workbook.CreateFont() as HSSFFont;
                font2.Boldweight = 400;
                font2.FontName = "宋体";
                style3.SetFont(font2);
                int num4 = 0;
                foreach (DataColumn column in dtData.Columns)
                {
                    HSSFCell cell = row4.CreateCell(column.Ordinal) as HSSFCell;
                    string str = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String":
                            cell.SetCellValue(str);
                            cell.CellStyle = style3;
                            goto Label_05D3;

                        case "System.DateTime":
                            DateTime time;
                            if (!DateTime.TryParse(str, out time))
                            {
                                break;
                            }
                            cell.SetCellValue(time);
                            goto Label_0502;

                        case "System.Boolean":
                            {
                                bool result = false;
                                bool.TryParse(str, out result);
                                cell.SetCellValue(result);
                                cell.CellStyle = style3;
                                goto Label_05D3;
                            }
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            {
                                int num5 = 0;
                                if (!int.TryParse(str, out num5))
                                {
                                    goto Label_055B;
                                }
                                cell.SetCellValue((double)num5);
                                goto Label_056A;
                            }
                        case "System.Decimal":
                        case "System.Double":
                            {
                                double num6 = 0.0;
                                double.TryParse(str, out num6);
                                cell.SetCellValue(num6);
                                cell.CellStyle = style3;
                                goto Label_05D3;
                            }
                        case "System.DBNull":
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            goto Label_05D3;

                        default:
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            goto Label_05D3;
                    }
                    cell.SetCellValue("");
                    Label_0502:
                    cell.CellStyle = style;
                    goto Label_05D3;
                    Label_055B:
                    cell.SetCellValue("");
                    Label_056A:
                    cell.CellStyle = style3;
                    Label_05D3:;
                }
                num4++;
                rownum++;
            }
            switch (rownum)
            {
                case 0xffff:
                case 0:
                    if (rownum != 0)
                    {
                        sheet = workbook.CreateSheet() as HSSFSheet;
                    }
                    row2 = sheet.CreateRow(0) as HSSFRow;
                    row2.HeightInPoints = 25f;
                    row2.CreateCell(0).SetCellValue(strHeaderText);
                    style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                    style2.Alignment = HorizontalAlignment.CENTER;
                    style2.BorderBottom = BorderStyle.THIN;
                    style2.BorderLeft = BorderStyle.THIN;
                    style2.BorderRight = BorderStyle.THIN;
                    style2.BorderTop = BorderStyle.THIN;
                    font = workbook.CreateFont() as HSSFFont;
                    font.FontHeightInPoints = 20;
                    font.Boldweight = 700;
                    font.FontName = "宋体";
                    style2.SetFont(font);
                    row2.GetCell(0).CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtData.Columns.Count - 1));
                    row2 = sheet.CreateRow(1) as HSSFRow;
                    row3 = sheet.CreateRow(2) as HSSFRow;
                    style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                    style2.Alignment = HorizontalAlignment.CENTER;
                    style2.BorderBottom = BorderStyle.THIN;
                    style2.BorderLeft = BorderStyle.THIN;
                    style2.BorderRight = BorderStyle.THIN;
                    style2.BorderTop = BorderStyle.THIN;
                    font = workbook.CreateFont() as HSSFFont;
                    font.FontName = "宋体";
                    style2.IsLocked = true;
                    style2.SetFont(font);
                    num2 = 0;
                    for (num3 = 0; num3 < dtHeader.Columns.Count; num3++)
                    {
                        row2.CreateCell(num2).SetCellValue(dtHeader.Columns[num3].ColumnName.ToString());
                        row2.GetCell(num2).CellStyle = style2;
                        num2++;
                    }
                    rownum = 2;
                    break;
            }
            HSSFCellStyle style4 = workbook.CreateCellStyle() as HSSFCellStyle;
            style4.Alignment = HorizontalAlignment.RIGHT;
            HSSFFont font3 = workbook.CreateFont() as HSSFFont;
            font3.Boldweight = 700;
            font3.FontName = "宋体";
            style4.SetFont(font3);
            HSSFRow row5 = sheet.CreateRow(rownum) as HSSFRow;
            CellRangeAddress region = new CellRangeAddress(rownum, rownum, 0, dtData.Columns.Count - 1);
            sheet.AddMergedRegion(region);
            row5.CreateCell(0).SetCellValue("合计 总金额: " + all.ToString() + "  未收款: " + no.ToString() + "  已收款: " + have.ToString());
            row5.GetCell(0).CellStyle = style4;
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);
                stream.Flush();
                stream.Position = 0L;
                return stream;
            }
        }

        public static MemoryStream exportMX(DataTable dtData, string strHeaderText, DataTable dtHeader, int times, int showcount)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
            DocumentSummaryInformation information = PropertySetFactory.CreateDocumentSummaryInformation();
            information.Company = "管理系统";
            workbook.DocumentSummaryInformation = information;
            SummaryInformation information2 = PropertySetFactory.CreateSummaryInformation();
            information2.Author = "Admin";
            information2.ApplicationName = "管理系统";
            information2.LastAuthor = "admin";
            information2.Comments = "管理系统";
            information2.Title = "管理系统";
            information2.Subject = "管理系统";
            information2.CreateDateTime = new DateTime?(DateTime.Now);
            workbook.SummaryInformation = information2;
            HSSFCellStyle style = workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            style.DataFormat = format.GetFormat("yyyy-mm-dd");
            style.Alignment = HorizontalAlignment.CENTER;
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            HSSFCellStyle style2 = workbook.CreateCellStyle() as HSSFCellStyle;
            style2.Alignment = HorizontalAlignment.CENTER;
            style2.BorderBottom = BorderStyle.THIN;
            style2.BorderLeft = BorderStyle.THIN;
            style2.BorderRight = BorderStyle.THIN;
            style2.BorderTop = BorderStyle.THIN;
            HSSFFont font = workbook.CreateFont() as HSSFFont;
            font.Boldweight = 400;
            font.FontName = "宋体";
            font.Color = HSSFColor.RED.index;
            style2.SetFont(font);
            HSSFCellStyle style3 = workbook.CreateCellStyle() as HSSFCellStyle;
            style3.Alignment = HorizontalAlignment.CENTER;
            style3.BorderBottom = BorderStyle.THIN;
            style3.BorderLeft = BorderStyle.THIN;
            style3.BorderRight = BorderStyle.THIN;
            style3.BorderTop = BorderStyle.THIN;
            HSSFFont font2 = workbook.CreateFont() as HSSFFont;
            font2.Boldweight = 400;
            font2.FontName = "宋体";
            font2.Color = HSSFColor.RED.index;
            style3.DataFormat = format.GetFormat("yyyy-mm-dd");
            style2.SetFont(font2);
            HSSFCellStyle style4 = workbook.CreateCellStyle() as HSSFCellStyle;
            style4.Alignment = HorizontalAlignment.CENTER;
            style4.BorderBottom = BorderStyle.THIN;
            style4.BorderLeft = BorderStyle.THIN;
            style4.BorderRight = BorderStyle.THIN;
            style4.BorderTop = BorderStyle.THIN;
            HSSFFont font3 = workbook.CreateFont() as HSSFFont;
            font3.Boldweight = 400;
            font3.FontName = "宋体";
            style4.SetFont(font3);
            int rownum = 0;
            int num2 = 0;
            foreach (DataRow row in dtData.Rows)
            {
                switch (rownum)
                {
                    case 0xffff:
                    case 0:
                        {
                            if (rownum != 0)
                            {
                                sheet = workbook.CreateSheet() as HSSFSheet;
                            }
                            HSSFRow row2 = sheet.CreateRow(0) as HSSFRow;
                            row2.HeightInPoints = 25f;
                            row2.CreateCell(0).SetCellValue(strHeaderText);
                            HSSFCellStyle style5 = workbook.CreateCellStyle() as HSSFCellStyle;
                            style5.Alignment = HorizontalAlignment.CENTER;
                            style5.BorderBottom = BorderStyle.THIN;
                            style5.BorderLeft = BorderStyle.THIN;
                            style5.BorderRight = BorderStyle.THIN;
                            style5.BorderTop = BorderStyle.THIN;
                            HSSFFont font4 = workbook.CreateFont() as HSSFFont;
                            font4.FontHeightInPoints = 20;
                            font4.Boldweight = 700;
                            font4.FontName = "宋体";
                            style5.SetFont(font4);
                            row2.GetCell(0).CellStyle = style5;
                            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, showcount));
                            row2 = sheet.CreateRow(1) as HSSFRow;
                            HSSFRow row3 = sheet.CreateRow(2) as HSSFRow;
                            style5 = workbook.CreateCellStyle() as HSSFCellStyle;
                            style5.Alignment = HorizontalAlignment.CENTER;
                            style5.BorderBottom = BorderStyle.THIN;
                            style5.BorderLeft = BorderStyle.THIN;
                            style5.BorderRight = BorderStyle.THIN;
                            style5.BorderTop = BorderStyle.THIN;
                            font4 = workbook.CreateFont() as HSSFFont;
                            font4.FontName = "宋体";
                            style5.IsLocked = true;
                            style5.SetFont(font4);
                            int num3 = 0;
                            for (int i = 0; i < (showcount + 1); i++)
                            {
                                row2.CreateCell(num3).SetCellValue(dtHeader.Columns[i].ColumnName.ToString());
                                row2.GetCell(num3).CellStyle = style5;
                                num3++;
                            }
                            rownum = 2;
                            break;
                        }
                }
                HSSFRow row4 = sheet.CreateRow(rownum) as HSSFRow;
                int whichCol = 0;
                foreach (DataColumn column in dtData.Columns)
                {
                    if (whichCol == (showcount + 1))
                    {
                        break;
                    }
                    HSSFCell cell = row4.CreateCell(column.Ordinal) as HSSFCell;
                    string str = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String":
                            cell.SetCellValue(str);
                            if (!(findTmOradress(whichCol, times, dtData.Rows[num2], "0") == "1"))
                            {
                                break;
                            }
                            cell.CellStyle = style2;
                            goto Label_0759;

                        case "System.DateTime":
                            DateTime time;
                            if (!DateTime.TryParse(str, out time))
                            {
                                goto Label_063C;
                            }
                            cell.SetCellValue(time);
                            goto Label_064B;

                        case "System.Boolean":
                            {
                                bool result = false;
                                bool.TryParse(str, out result);
                                cell.SetCellValue(result);
                                cell.CellStyle = style4;
                                goto Label_0759;
                            }
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            {
                                int num6 = 0;
                                if (!int.TryParse(str, out num6))
                                {
                                    goto Label_06E1;
                                }
                                cell.SetCellValue((double)num6);
                                goto Label_06F0;
                            }
                        case "System.Decimal":
                        case "System.Double":
                            {
                                double num7 = 0.0;
                                double.TryParse(str, out num7);
                                cell.SetCellValue(num7);
                                cell.CellStyle = style4;
                                goto Label_0759;
                            }
                        case "System.DBNull":
                            cell.SetCellValue("");
                            cell.CellStyle = style4;
                            goto Label_0759;

                        default:
                            cell.SetCellValue("");
                            cell.CellStyle = style4;
                            goto Label_0759;
                    }
                    cell.CellStyle = style4;
                    goto Label_0759;
                    Label_063C:
                    cell.SetCellValue("");
                    Label_064B:
                    if (findTmOradress(whichCol, times, dtData.Rows[num2], "1") == "1")
                    {
                        cell.CellStyle = style2;
                    }
                    else
                    {
                        cell.CellStyle = style;
                    }
                    goto Label_0759;
                    Label_06E1:
                    cell.SetCellValue("");
                    Label_06F0:
                    cell.CellStyle = style4;
                    Label_0759:
                    whichCol++;
                }
                rownum++;
                num2++;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);
                stream.Flush();
                stream.Position = 0L;
                return stream;
            }
        }

        public static MemoryStream exportPro(DataTable dtData, string strHeaderText, DataTable dtHeader, string EndTitle)
        {
            HSSFRow row2;
            HSSFCellStyle style2;
            HSSFFont font;
            HSSFRow row3;
            int num3;
            int num4;
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
            DocumentSummaryInformation information = PropertySetFactory.CreateDocumentSummaryInformation();
            information.Company = "管理系统";
            workbook.DocumentSummaryInformation = information;
            SummaryInformation information2 = PropertySetFactory.CreateSummaryInformation();
            information2.Author = "Admin";
            information2.ApplicationName = "管理系统";
            information2.LastAuthor = "admin";
            information2.Comments = "管理系统";
            information2.Title = "管理系统";
            information2.Subject = "管理系统";
            information2.CreateDateTime = new DateTime?(DateTime.Now);
            workbook.SummaryInformation = information2;
            HSSFCellStyle style = workbook.CreateCellStyle() as HSSFCellStyle;
            style.DataFormat = (workbook.CreateDataFormat() as HSSFDataFormat).GetFormat("yyyy-mm-dd");
            style.Alignment = HorizontalAlignment.CENTER;
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            int rownum = 0;
            int num2 = 0;
            foreach (DataRow row in dtData.Rows)
            {
                if (num2 == (dtData.Rows.Count))
                {
                    break;
                }
                num2++;
                switch (rownum)
                {
                    case 0xffff:
                    case 0:
                        if (rownum != 0)
                        {
                            sheet = workbook.CreateSheet() as HSSFSheet;
                        }
                        row2 = sheet.CreateRow(0) as HSSFRow;
                        row2.HeightInPoints = 25f;
                        row2.CreateCell(0).SetCellValue(strHeaderText);
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        font.FontName = "宋体";
                        style2.SetFont(font);
                        row2.GetCell(0).CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtData.Columns.Count));
                        row2 = sheet.CreateRow(1) as HSSFRow;
                        row3 = sheet.CreateRow(2) as HSSFRow;
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontName = "宋体";
                        style2.IsLocked = true;
                        style2.SetFont(font);
                        num3 = 0;
                        num4 = 0;
                        while (num4 < dtHeader.Columns.Count)
                        {
                            row2.CreateCell(num3).SetCellValue(dtHeader.Columns[num4].ColumnName.ToString());
                            row2.GetCell(num3).CellStyle = style2;
                            num3++;
                            num4++;
                        }
                        rownum = 2;
                        break;
                }
                HSSFRow row4 = sheet.CreateRow(rownum) as HSSFRow;
                HSSFCellStyle style3 = workbook.CreateCellStyle() as HSSFCellStyle;
                style3.Alignment = HorizontalAlignment.CENTER;
                style3.BorderBottom = BorderStyle.THIN;
                style3.BorderLeft = BorderStyle.THIN;
                style3.BorderRight = BorderStyle.THIN;
                style3.BorderTop = BorderStyle.THIN;
                HSSFFont font2 = workbook.CreateFont() as HSSFFont;
                font2.Boldweight = 400;
                font2.FontName = "宋体";
                style3.SetFont(font2);
                int num5 = 0;
                foreach (DataColumn column in dtData.Columns)
                {
                    HSSFCell cell = row4.CreateCell(column.Ordinal) as HSSFCell;
                    string str = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String":
                            cell.SetCellValue(str);
                            cell.CellStyle = style3;
                            goto Label_05E7;

                        case "System.DateTime":
                            DateTime time;
                            if (!DateTime.TryParse(str, out time))
                            {
                                break;
                            }
                            cell.SetCellValue(time);
                            goto Label_052B;

                        case "System.Boolean":
                            {
                                bool result = false;
                                bool.TryParse(str, out result);
                                cell.SetCellValue(result);
                                cell.CellStyle = style3;
                                goto Label_05E7;
                            }
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            {
                                int num6 = 0;
                                if (!int.TryParse(str, out num6))
                                {
                                    goto Label_0584;
                                }
                                cell.SetCellValue((double)num6);
                                goto Label_0593;
                            }
                        case "System.Decimal":
                        case "System.Double":
                            cell.SetCellValue(str);
                            cell.CellStyle = style3;
                            goto Label_05E7;

                        case "System.DBNull":
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            goto Label_05E7;

                        default:
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            goto Label_05E7;
                    }
                    cell.SetCellValue("");
                    Label_052B:
                    cell.CellStyle = style;
                    goto Label_05E7;
                    Label_0584:
                    cell.SetCellValue("");
                    Label_0593:
                    cell.CellStyle = style3;
                    Label_05E7:;
                }
                num5++;
                rownum++;
            }
            if ((num2 + 1) == dtData.Rows.Count)
            {
                switch (rownum)
                {
                    case 0xffff:
                    case 0:
                        if (rownum != 0)
                        {
                            sheet = workbook.CreateSheet() as HSSFSheet;
                        }
                        row2 = sheet.CreateRow(0) as HSSFRow;
                        row2.HeightInPoints = 25f;
                        row2.CreateCell(0).SetCellValue(strHeaderText);
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        font.FontName = "宋体";
                        style2.SetFont(font);
                        row2.GetCell(0).CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtData.Columns.Count - 1));
                        row2 = sheet.CreateRow(1) as HSSFRow;
                        row3 = sheet.CreateRow(2) as HSSFRow;
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontName = "宋体";
                        style2.IsLocked = true;
                        style2.SetFont(font);
                        num3 = 0;
                        for (num4 = 0; num4 < dtHeader.Columns.Count; num4++)
                        {
                            row2.CreateCell(num3).SetCellValue(dtHeader.Columns[num4].ColumnName.ToString());
                            row2.GetCell(num3).CellStyle = style2;
                            num3++;
                        }
                        rownum = 2;
                        break;
                }
                HSSFCellStyle style4 = workbook.CreateCellStyle() as HSSFCellStyle;
                style4.Alignment = HorizontalAlignment.RIGHT;
                HSSFFont font3 = workbook.CreateFont() as HSSFFont;
                font3.Boldweight = 700;
                font3.FontName = "宋体";
                style4.SetFont(font3);
                HSSFRow row5 = sheet.CreateRow(rownum) as HSSFRow;
                CellRangeAddress region = new CellRangeAddress(rownum, rownum, 0, dtData.Columns.Count - 1);
                sheet.AddMergedRegion(region);
                row5.CreateCell(0).SetCellValue(EndTitle);
                row5.GetCell(0).CellStyle = style4;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);
                stream.Flush();
                stream.Position = 0L;
                return stream;
            }
        }



        public static MemoryStream portProEx(DataTable dtData, string strHeaderText, DataTable dtHeader, string EndTitle)
        {
            HSSFRow row2;
            HSSFCellStyle style2;
            HSSFFont font;
            HSSFRow row3;
            int num3;
            int num4;
            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet() as HSSFSheet;
            DocumentSummaryInformation information = PropertySetFactory.CreateDocumentSummaryInformation();
            information.Company = "管理系统";
            workbook.DocumentSummaryInformation = information;
            SummaryInformation information2 = PropertySetFactory.CreateSummaryInformation();
            information2.Author = "Admin";
            information2.ApplicationName = "管理系统";
            information2.LastAuthor = "admin";
            information2.Comments = "管理系统";
            information2.Title = "管理系统";
            information2.Subject = "管理系统";
            information2.CreateDateTime = new DateTime?(DateTime.Now);
            workbook.SummaryInformation = information2;
            HSSFCellStyle style = workbook.CreateCellStyle() as HSSFCellStyle;
            style.DataFormat = (workbook.CreateDataFormat() as HSSFDataFormat).GetFormat("yyyy-mm-dd");
            style.Alignment = HorizontalAlignment.CENTER;
            style.BorderBottom = BorderStyle.THIN;
            style.BorderLeft = BorderStyle.THIN;
            style.BorderRight = BorderStyle.THIN;
            style.BorderTop = BorderStyle.THIN;
            int rownum = 0;
            int num2 = 0;
            foreach (DataRow row in dtData.Rows)
            {
                if (num2 == (dtData.Rows.Count))
                {
                    break;
                }
                num2++;
                switch (rownum)
                {
                    case 0xffff:
                    case 0:
                        if (rownum != 0)
                        {
                            sheet = workbook.CreateSheet() as HSSFSheet;
                        }
                        row2 = sheet.CreateRow(0) as HSSFRow;
                        row2.HeightInPoints = 25f;
                        row2.CreateCell(0).SetCellValue(strHeaderText);
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        font.FontName = "宋体";
                        style2.SetFont(font);
                        row2.GetCell(0).CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtData.Columns.Count - 1));
                        row2 = sheet.CreateRow(1) as HSSFRow;
                        row3 = sheet.CreateRow(2) as HSSFRow;
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontName = "宋体";
                        style2.IsLocked = true;
                        style2.SetFont(font);
                        num3 = 0;
                        num4 = 0;
                        while (num4 < dtData.Columns.Count)
                        {
                            row2.CreateCell(num3).SetCellValue(dtData.Columns[num4].ColumnName.ToString());
                            row2.GetCell(num3).CellStyle = style2;
                            num3++;
                            num4++;
                        }
                        rownum = 2;
                        break;
                }
                HSSFRow row4 = sheet.CreateRow(rownum) as HSSFRow;
                HSSFCellStyle style3 = workbook.CreateCellStyle() as HSSFCellStyle;
                style3.Alignment = HorizontalAlignment.CENTER;
                style3.BorderBottom = BorderStyle.THIN;
                style3.BorderLeft = BorderStyle.THIN;
                style3.BorderRight = BorderStyle.THIN;
                style3.BorderTop = BorderStyle.THIN;
                HSSFFont font2 = workbook.CreateFont() as HSSFFont;
                font2.Boldweight = 400;
                font2.FontName = "宋体";
                style3.SetFont(font2);
                int num5 = 0;
                foreach (DataColumn column in dtData.Columns)
                {
                    HSSFCell cell = row4.CreateCell(column.Ordinal) as HSSFCell;
                    string str = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String":
                            cell.SetCellValue(str);
                            cell.CellStyle = style3;
                            goto Label_05E7;

                        case "System.DateTime":
                            DateTime time;
                            if (!DateTime.TryParse(str, out time))
                            {
                                break;
                            }
                            cell.SetCellValue(time);
                            goto Label_052B;

                        case "System.Boolean":
                            {
                                bool result = false;
                                bool.TryParse(str, out result);
                                cell.SetCellValue(result);
                                cell.CellStyle = style3;
                                goto Label_05E7;
                            }
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            {
                                int num6 = 0;
                                if (!int.TryParse(str, out num6))
                                {
                                    goto Label_0584;
                                }
                                cell.SetCellValue((double)num6);
                                goto Label_0593;
                            }
                        case "System.Decimal":
                        case "System.Double":
                            cell.SetCellValue(str);
                            cell.CellStyle = style3;
                            goto Label_05E7;

                        case "System.DBNull":
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            goto Label_05E7;

                        default:
                            cell.SetCellValue("");
                            cell.CellStyle = style3;
                            goto Label_05E7;
                    }
                    cell.SetCellValue("");
                    Label_052B:
                    cell.CellStyle = style;
                    goto Label_05E7;
                    Label_0584:
                    cell.SetCellValue("");
                    Label_0593:
                    cell.CellStyle = style3;
                    Label_05E7:;
                }
                num5++;
                rownum++;
            }
            if ((num2 + 1) == dtData.Rows.Count)
            {
                switch (rownum)
                {
                    case 0xffff:
                    case 0:
                        if (rownum != 0)
                        {
                            sheet = workbook.CreateSheet() as HSSFSheet;
                        }
                        row2 = sheet.CreateRow(0) as HSSFRow;
                        row2.HeightInPoints = 25f;
                        row2.CreateCell(0).SetCellValue(strHeaderText);
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        font.FontName = "宋体";
                        style2.SetFont(font);
                        row2.GetCell(0).CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtData.Columns.Count - 1));
                        row2 = sheet.CreateRow(1) as HSSFRow;
                        row3 = sheet.CreateRow(2) as HSSFRow;
                        style2 = workbook.CreateCellStyle() as HSSFCellStyle;
                        style2.Alignment = HorizontalAlignment.CENTER;
                        style2.BorderBottom = BorderStyle.THIN;
                        style2.BorderLeft = BorderStyle.THIN;
                        style2.BorderRight = BorderStyle.THIN;
                        style2.BorderTop = BorderStyle.THIN;
                        font = workbook.CreateFont() as HSSFFont;
                        font.FontName = "宋体";
                        style2.IsLocked = true;
                        style2.SetFont(font);
                        num3 = 0;
                        for (num4 = 0; num4 < dtHeader.Columns.Count; num4++)
                        {
                            row2.CreateCell(num3).SetCellValue(dtHeader.Columns[num4].ColumnName.ToString());
                            row2.GetCell(num3).CellStyle = style2;
                            num3++;
                        }
                        rownum = 2;
                        break;
                }
                HSSFCellStyle style4 = workbook.CreateCellStyle() as HSSFCellStyle;
                style4.Alignment = HorizontalAlignment.RIGHT;
                HSSFFont font3 = workbook.CreateFont() as HSSFFont;
                font3.Boldweight = 700;
                font3.FontName = "宋体";
                style4.SetFont(font3);
                HSSFRow row5 = sheet.CreateRow(rownum) as HSSFRow;
                CellRangeAddress region = new CellRangeAddress(rownum, rownum, 0, dtData.Columns.Count - 1);
                sheet.AddMergedRegion(region);
                row5.CreateCell(0).SetCellValue(EndTitle);
                row5.GetCell(0).CellStyle = style4;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.Write(stream);
                stream.Flush();
                stream.Position = 0L;
                return stream;
            }
        }

        public static string findTmOradress(int whichCol, int times, DataRow dr, string type)
        {
            string str = "0";
            if (type == "0")
            {
                if (times == 2)
                {
                    switch (whichCol)
                    {
                        case 4:
                            switch (dr["DKStart1"].ToString())
                            {
                                case "1":
                                    str = "1";
                                    break;

                                case "3":
                                    str = "1";
                                    break;
                            }
                            break;

                        case 5:
                            switch (dr["DKStart1"].ToString())
                            {
                                case "2":
                                    str = "1";
                                    break;

                                case "3":
                                    str = "1";
                                    break;
                            }
                            break;

                        case 6:
                            switch (dr["DKStart2"].ToString())
                            {
                                case "1":
                                    str = "1";
                                    break;

                                case "3":
                                    str = "1";
                                    break;
                            }
                            break;

                        case 7:
                            switch (dr["DKStart2"].ToString())
                            {
                                case "2":
                                    str = "1";
                                    break;

                                case "3":
                                    str = "1";
                                    break;
                            }
                            break;
                    }
                }
                if (times == 4)
                {
                    switch (whichCol)
                    {
                        case 4:
                            switch (dr["DKStart1"].ToString())
                            {
                                case "1":
                                    return "1";

                                case "3":
                                    return "1";
                            }
                            return str;

                        case 5:
                            switch (dr["DKStart1"].ToString())
                            {
                                case "2":
                                    return "1";

                                case "3":
                                    return "1";
                            }
                            return str;

                        case 6:
                            switch (dr["DKStart2"].ToString())
                            {
                                case "1":
                                    return "1";

                                case "3":
                                    return "1";
                            }
                            return str;

                        case 7:
                            switch (dr["DKStart2"].ToString())
                            {
                                case "2":
                                    return "1";

                                case "3":
                                    return "1";
                            }
                            return str;

                        case 8:
                            switch (dr["DKStart3"].ToString())
                            {
                                case "1":
                                    return "1";

                                case "3":
                                    return "1";
                            }
                            return str;

                        case 9:
                            switch (dr["DKStart3"].ToString())
                            {
                                case "2":
                                    return "1";

                                case "3":
                                    return "1";
                            }
                            return str;

                        case 10:
                            switch (dr["DKStart4"].ToString())
                            {
                                case "1":
                                    return "1";

                                case "3":
                                    return "1";
                            }
                            return str;

                        case 11:
                            switch (dr["DKStart4"].ToString())
                            {
                                case "2":
                                    return "1";

                                case "3":
                                    return "1";
                            }
                            return str;
                    }
                }
            }
            return str;
        }

        public static DataTable getDataTableDaoRu(string fullpath)
        {
            int num2;
            FileStream s = File.Open(fullpath, FileMode.Open, FileAccess.Read);
            ISheet sheetAt = new HSSFWorkbook(s).GetSheetAt(0);
            DataTable table = new DataTable();
            IRow row = sheetAt.GetRow(1);
            int lastCellNum = row.LastCellNum;
            for (num2 = row.FirstCellNum; num2 < lastCellNum; num2++)
            {
                DataColumn column = new DataColumn(row.GetCell(num2).StringCellValue);
                table.Columns.Add(column);
            }
            int lastRowNum = sheetAt.LastRowNum;
            int num4 = 0;
            IRow row2 = sheetAt.GetRow(0);
            IRow row3 = sheetAt.GetRow(1);
            string str = row2.GetCell(0).ToString();
            string str2 = row3.GetCell(0).ToString();
            string str3 = row3.GetCell(1).ToString();
            if (((str != "公海池管理表") || (str2 != " 序号*")) || (str3 != "客户名称*"))
            {
                return null;
            }
            for (num2 = sheetAt.FirstRowNum + 2; num2 < (sheetAt.LastRowNum + 1); num2++)
            {
                num4 = 0;
                IRow row4 = sheetAt.GetRow(num2);
                if (row4 != null)
                {
                    DataRow row5 = table.NewRow();
                    for (int i = 0; i < lastCellNum; i++)
                    {
                        if (row4.GetCell(i) != null)
                        {
                            if (row4.GetCell(i).ToString().Trim() != "")
                            {
                                row5[i] = row4.GetCell(i).ToString();
                            }
                            else
                            {
                                num4++;
                            }
                        }
                        else
                        {
                            num4++;
                        }
                    }
                    if (num4 < 9)
                    {
                        table.Rows.Add(row5);
                    }
                }
            }
            s.Close();
            File.Delete(fullpath);
            return table;
        }

        public static DataTable GetImport(string fullpath)
        {
            int num2;
            FileStream s = File.Open(fullpath, FileMode.Open, FileAccess.Read);
            ISheet sheetAt = new HSSFWorkbook(s).GetSheetAt(0);
            DataTable table = new DataTable();
            IRow row = sheetAt.GetRow(1);
            int lastCellNum = row.LastCellNum;
            for (num2 = row.FirstCellNum; num2 < lastCellNum; num2++)
            {
                DataColumn column = new DataColumn(row.GetCell(num2).StringCellValue);
                table.Columns.Add(column);
            }
            int lastRowNum = sheetAt.LastRowNum;
            int num4 = 0;
            IRow row2 = sheetAt.GetRow(0);
            IRow row3 = sheetAt.GetRow(1);
            string str = row2.GetCell(0).ToString();
            string str2 = row3.GetCell(0).ToString();
            string str3 = row3.GetCell(1).ToString();
            if (((str != "客户管理表") || (str2 != " 序号")) || (str3 != "客户名称*"))
            {
                return null;
            }
            for (num2 = sheetAt.FirstRowNum + 2; num2 < (sheetAt.LastRowNum + 1); num2++)
            {
                num4 = 0;
                IRow row4 = sheetAt.GetRow(num2);
                if (row4 != null)
                {
                    DataRow row5 = table.NewRow();
                    for (int i = 0; i < lastCellNum; i++)
                    {
                        if (row4.GetCell(i) != null)
                        {
                            if (row4.GetCell(i).ToString().Trim() != "")
                            {
                                row5[i] = row4.GetCell(i).ToString();
                            }
                            else
                            {
                                num4++;
                            }
                        }
                        else
                        {
                            num4++;
                        }
                    }
                    if (num4 < 9)
                    {
                        table.Rows.Add(row5);
                    }
                }
            }
            s.Close();
            File.Delete(fullpath);
            return table;
        }

        public static DataTable GetImportUser(string fullpath)
        {
            int num2;
            FileStream s = File.Open(fullpath, FileMode.Open, FileAccess.Read);
            ISheet sheetAt = new HSSFWorkbook(s).GetSheetAt(0);
            DataTable table = new DataTable();
            IRow row = sheetAt.GetRow(1);
            int lastCellNum = row.LastCellNum;
            for (num2 = row.FirstCellNum; num2 < lastCellNum; num2++)
            {
                DataColumn column = new DataColumn(row.GetCell(num2).StringCellValue);
                table.Columns.Add(column);
            }
            int lastRowNum = sheetAt.LastRowNum;
            int num4 = 0;
            IRow row2 = sheetAt.GetRow(0);
            IRow row3 = sheetAt.GetRow(1);
            string str = row2.GetCell(0).ToString();
            string str2 = row3.GetCell(0).ToString();
            string str3 = row3.GetCell(1).ToString();
            if (((str != "联系人管理表") || (str2 != " 序号")) || (str3 != "姓名*"))
            {
                return null;
            }
            for (num2 = sheetAt.FirstRowNum + 2; num2 < (sheetAt.LastRowNum + 1); num2++)
            {
                num4 = 0;
                IRow row4 = sheetAt.GetRow(num2);
                if (row4 != null)
                {
                    DataRow row5 = table.NewRow();
                    for (int i = 0; i < lastCellNum; i++)
                    {
                        if (row4.GetCell(i) != null)
                        {
                            if (row4.GetCell(i).ToString().Trim() != "")
                            {
                                row5[i] = row4.GetCell(i).ToString();
                            }
                            else
                            {
                                num4++;
                            }
                        }
                        else
                        {
                            num4++;
                        }
                    }
                    if (num4 < 7)
                    {
                        table.Rows.Add(row5);
                    }
                }
            }
            s.Close();
            File.Delete(fullpath);
            return table;
        }

        public static DataTable Import(string strFileName)
        {
            HSSFWorkbook workbook;
            DataTable table = new DataTable();
            using (FileStream stream = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(stream);
            }
            HSSFSheet sheetAt = workbook.GetSheetAt(0) as HSSFSheet;
            IEnumerator rowEnumerator = sheetAt.GetRowEnumerator();
            HSSFRow row = sheetAt.GetRow(0) as HSSFRow;
            int lastCellNum = row.LastCellNum;
            int cellnum = 0;
            while (cellnum < lastCellNum)
            {
                HSSFCell cell = row.GetCell(cellnum) as HSSFCell;
                table.Columns.Add(cell.ToString());
                cellnum++;
            }
            for (int i = sheetAt.FirstRowNum + 1; i <= sheetAt.LastRowNum; i++)
            {
                HSSFRow row2 = sheetAt.GetRow(i) as HSSFRow;
                DataRow row3 = table.NewRow();
                for (cellnum = row2.FirstCellNum; cellnum < lastCellNum; cellnum++)
                {
                    if (row2.GetCell(cellnum) != null)
                    {
                        row3[cellnum] = row2.GetCell(cellnum).ToString();
                    }
                }
                table.Rows.Add(row3);
            }
            return table;
        }
    }
}

