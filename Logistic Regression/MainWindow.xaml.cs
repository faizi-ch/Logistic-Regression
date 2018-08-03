using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using LumenWorks.Framework.IO.Csv;

namespace Logistic_Regression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXWindow
    {
        private int n = 0, m = 0;
        private double[,] xFeatures;
        private double alpha = 0;
        private string f = "";
        private int x = 0, y = 0, t = 0, total = 0, noOfFeatures=0, features=0;
        List<String> xList1=new List<string>();
        List<String> xList2 = new List<string>();
        List<String> xList3 = new List<string>();
        List<String> xList4 = new List<string>();
        List<String> xList5 = new List<string>();
        List<String> xList6 = new List<string>();
        List<String> xList7 = new List<string>();
        List<String> xList8 = new List<string>();
        List<String> xList9 = new List<string>();
    
        public MainWindow()
        {
            InitializeComponent();
            total = 9;
            XList.Items.Add("x(1)");
        }

        private void XTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (XTextEdit.Text != "")
                {
                    XList.Items[m+1] += f+XTextEdit.Text;

                    xFeatures[n, m] = Convert.ToDouble(XTextEdit.Text);
                    m++;
                    if (total == m && noOfFeatures == n + 1)
                    {
                        XTextEdit.IsEnabled = false;
                        YTextEdit.Focus();
                    }
                    if (m==total)
                    {
                        m = 0;
                        n++;

                        if(n!=noOfFeatures)
                            XList.Items[0] += string.Format("\tx({0})", n + 1);

                        if (n == 1)
                            f += "\t";
                    }
                }
                XTextEdit.Text = "";
            }
        }

        private void YTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (YTextEdit.Text != "")
                {
                    YList.Items.Add(YTextEdit.Text);
                }
                y++;
                YTextEdit.Text = "";
            }
            if (total == y)
            {
                YTextEdit.IsEnabled = false;
            }
        }

        private void ThetaTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ThetaTextEdit.Text != "")
                {
                    ThetasList.Items.Add(ThetaTextEdit.Text);
                }
                t++;
                ThetaTextEdit.Text = "";
                ThetaLabel.Content = string.Format("θ({0}): ", t);
            }
            if (noOfFeatures == t)
            {
                ThetaTextEdit.IsEnabled = false;
                XTextEdit.Focus();
            }
        }

        private void AlphaTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (AlphaTextEdit.Text != "")
            {
                alpha = Convert.ToDouble(AlphaTextEdit.Text);
            }
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (
                CsvReader csvReader =
                    new CsvReader(
                        new StreamReader(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName +
                                         @"\biopsy.csv"), hasHeaders: true))
            {

                while (csvReader.ReadNextRecord())
                {

                    xList1.Add(csvReader[2]);
                    xList2.Add(csvReader[3]);
                    xList3.Add(csvReader[4]);
                    xList4.Add(csvReader[5]);
                    xList5.Add(csvReader[6]);
                    xList6.Add(csvReader[7]);
                    xList7.Add(csvReader[8]);
                    xList8.Add(csvReader[9]);
                    xList9.Add(csvReader[10]);

                }

                DataTable dt = new DataTable();
                dt.Columns.Add("x1");
                dt.Columns.Add("x2");
                dt.Columns.Add("x3");
                dt.Columns.Add("x4");
                dt.Columns.Add("x5");
                dt.Columns.Add("x6");
                dt.Columns.Add("x7");
                dt.Columns.Add("x8");
                dt.Columns.Add("x9");
                for (int i = 0; i < xList1.Count; i++)
                {
                    //var row = dt.NewRow();
                    object[] RowValues = { xList1[i], xList2[i], xList3[i], xList4[i], xList5[i], xList6[i], xList7[i], xList8[i], xList9[i]};
                    /*row["x1"] = xList1[i];
                    row["x2"] = xList2[i];
                    row["x3"] = xList3[i];
                    row["x4"] = xList4[i];
                    row["x5"] = xList5[i];
                    row["x6"] = xList6[i];
                    row["x7"] = xList7[i];
                    row["x8"] = xList8[i];
                    row["x9"] = xList9[i];*/
                    DataRow dRow;
                    dRow = dt.Rows.Add(RowValues);
                    dt.AcceptChanges();
                    
                }

                DataGrid.DataSource = dt;
                


                xFeatures =new double[xList1.Count,9];
                //DataGrid.Columns.Add(new GridColumn { Header = "x1", Binding=new Binding("xList1")});
                for (int i = 0; i < xList1.Count; i++)
                {
                    xFeatures[i, 0] = Convert.ToDouble(xList1[i]);

                    
                }
                
                for (int i = 0; i < xList1.Count; i++)
                {
                    xFeatures[i, 1] = Convert.ToDouble(xList2[i]);
                }
                for (int i = 0; i < xList1.Count; i++)
                {
                    xFeatures[i, 2] = Convert.ToDouble(xList3[i]);
                }
                for (int i = 0; i < xList1.Count; i++)
                {
                    xFeatures[i, 3] = Convert.ToDouble(xList4[i]);
                }
                for (int i = 0; i < xList1.Count; i++)
                {
                    xFeatures[i, 4] = Convert.ToDouble(xList5[i]);
                }
                for (int i = 0; i < xList1.Count; i++)
                {
                    try
                    {
                        xFeatures[i, 5] = Convert.ToDouble(xList6[i]);
                    }
                    catch (FormatException exception)
                    {
                        xFeatures[i, 5] = 0;
                        
                    }
                    
                }
                for (int i = 0; i < xList1.Count; i++)
                {
                    xFeatures[i, 6] = Convert.ToDouble(xList7[i]);
                }
                for (int i = 0; i < xList1.Count; i++)
                {
                    xFeatures[i, 7] = Convert.ToDouble(xList8[i]);
                }
                for (int i = 0; i < xList1.Count; i++)
                {
                    xFeatures[i, 8] = Convert.ToDouble(xList9[i]);
                }

                for (int i = 0; i < xList1.Count; i++)
                {
                    for (int j = 0; j < xList1.Count; j++)
                    {
                        Console.Write(xFeatures[i,j]);
                    }
                    Console.WriteLine();9
                }
            }
            AlphaTextEdit.Focus();
        }
            
        

        private void TotalTextEdit_EditValueChanging(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
        {
            if (TotalTextEdit.Text != "")
            {
                total = Convert.ToInt32(TotalTextEdit.Text);
            }
            if (total>0)
            {
                XTextEdit.IsEnabled = true;
                YTextEdit.IsEnabled = true;
                ThetaTextEdit.IsEnabled = true;
            }
        }

        private void TotalTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (TotalTextEdit.Text != "")
            {
                total = Convert.ToInt32(TotalTextEdit.Text);
                for (int i = 0; i < total; i++)
                {
                    XList.Items.Add("");
                }
                if (NTextEdit.Text!= "")
                {
                    xFeatures = new double[noOfFeatures, total];
                }
            }
            if (total > 0)
            {
                XTextEdit.IsEnabled = true;
                YTextEdit.IsEnabled = true;
                ThetaTextEdit.IsEnabled = true;
            }
            if (total == 0)
            {
                XTextEdit.IsEnabled = false;
                YTextEdit.IsEnabled = false;
                ThetaTextEdit.IsEnabled = false;
            }
        }

        private void NTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (NTextEdit.Text!="")
            {
                noOfFeatures = Convert.ToInt32(NTextEdit.Text);

                if (TotalTextEdit.Text != "")
                {
                    xFeatures = new double[noOfFeatures, total];
                }
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double[] oldThetas;
            double[] updatedThetas = new double[ThetasList.Items.Count];
            if (NTextEdit.Text != "" && TotalTextEdit.Text != "" && XList.Items.Count > 0 && YList.Items.Count > 0)
            {
                oldThetas = new double[ThetasList.Items.Count];
                

                //newThetas[0] = Convert.ToDouble(ThetasList.Items[0]) - (alpha * (CalculateCostFunctionForTheta0(i)));

                for (int i = 0; i < 10; i++)
                {
                    UpdatedThetasList.Items.Add("");

                    for (int j = 0; j < ThetasList.Items.Count; j++)
                    {
                        if (j == 0)
                            updatedThetas[j] = oldThetas[j] - (alpha * (CalculateCostFunctionForTheta0(oldThetas)));
                        else
                            updatedThetas[j] = oldThetas[j] - (alpha * (CalculateCostFunctionForTheta1(oldThetas, j)));

                        /*if (t0 < 0 || t1 < 0)
                        {
                            break;
                        }*/
                        if (j == 0)
                            UpdatedThetasList.Items[i] += string.Format("{0:F4}", updatedThetas[j]);
                        else
                            UpdatedThetasList.Items[i] += string.Format("\t{0:F4}", updatedThetas[j]);

                        oldThetas[j] = updatedThetas[j];
                    }
                    
                }
            }
        }
        private double CalculateCostFunctionForTheta0(double[] thetas)
        {
            double h = 0, j = 0, g = 0;
            int i = 0;
            if (ThetasList.Items.Count > 0 && XList.Items.Count > 0 && YList.Items.Count > 0)
            {
                //h += thetas[0];
                for (int n = 0; n < noOfFeatures; n++)
                {
                    for (int m = 0; m < total; m++)
                    {
                        h += thetas[n] * xFeatures[n, m];
                    }
                }
                g = 1 / (1 + Math.Pow(Math.E, -h));

                foreach (var y in YList.Items)
                {
                    j += Convert.ToDouble(y) * Math.Log10(h) + (1 - Convert.ToDouble(y)*Math.Log10(1-h));
                    //MessageBox.Show(j.ToString());
                    CalculatedList.Items.Add(j.ToString());
                    i++;
                }
                
                
                //MessageBox.Show(j.ToString());
                //double m = (2 * total);
                double d = -1 / (double)total;
                j *= d;

                //CalculatedLabel.Content += j.ToString();
            }
   
            return j;
        }

        private double CalculateCostFunctionForTheta1(double[] thetas, int f)
        {
            double h = 0, j = 0;
            int i = 0;
            if (ThetasList.Items.Count > 0 && XList.Items.Count > 0 && YList.Items.Count > 0)
            {
                //h += thetas[0];
                for (int n = 0; n < noOfFeatures; n++)
                {
                    for (int m = 0; m < total; m++)
                    {
                        h += thetas[n] * xFeatures[n, m];
                    }
                }

                foreach (var y in YList.Items)
                {
                    j += h - Convert.ToDouble(y);
                    j *= xFeatures[f, i];
                    CalculatedList.Items.Add(j.ToString());
                    i++;
                }


                //MessageBox.Show(j.ToString());
                //double m = (2 * total);
                double d = 1 / (double)total;
                j *= d;

                //CalculatedLabel.Content += j.ToString();
            }

            return j;
        }
    }
}
