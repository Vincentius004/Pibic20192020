﻿using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace apSerial
{
    public partial class frmSerial : Form
    {
        public frmSerial()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // CRIA O ARQUIVO
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);

                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                // COLOCA AS COISA NO EXCEL
                Microsoft.Office.Interop.Excel.Application XcelApp = new Microsoft.Office.Interop.Excel.Application();
                if (dgv.Rows.Count > 0)
                {
                    try
                    {
                        XcelApp.Application.Workbooks.Add(Type.Missing);
                        for (int i = 1; i < dgv.Columns.Count + 1; i++)
                        {
                            XcelApp.Cells[1, i] = dgv.Columns[i - 1].HeaderText;
                        }
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            for (int j = 0; j < dgv.Columns.Count; j++)
                            {
                                XcelApp.Cells[i + 2, j + 1] = dgv.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                        XcelApp.Columns.AutoFit();
                        XcelApp.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro : " + ex.Message);
                        XcelApp.Quit();
                    }
                }

                // E SALVA
                // colocar o path inteiro + txtArquivoExcel.Text;
                xlWorkBook.SaveAs(txtArquivoExcel.Text, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
 Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                liberarObjetos(xlWorkSheet);
                liberarObjetos(xlWorkBook);
                liberarObjetos(xlApp);

                MessageBox.Show("O arquivo Excel foi criado com sucesso. Você pode encontrá-lo em: " + txtArquivoExcel.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message);
            }

        }

        private void liberarObjetos(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Ocorreu um erro durante a liberação do objeto " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
