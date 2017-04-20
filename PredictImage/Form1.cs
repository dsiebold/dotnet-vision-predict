using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace PredictImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             //var imageUrl = "http://metamind.io/images/generalimage.jpg";
            var imageUrl = lblUrl.Text;
            pcbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            pcbImage.Load(imageUrl);
            ResetChart();
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            try
            {
                var authToken = "replace_with_your_token";
                var modelId = "GeneralImageClassifier";
                var sampleLocation = lblUrl.Text;
                var baseURL = "https://api.metamind.io/v1/vision/predict";

                // Create the web request
                var request = (HttpWebRequest)WebRequest.Create(baseURL);
                request.Headers.Add("Authorization", "Bearer " + authToken);
                request.Method = "POST";
                request.Accept = "application/json";
                var boundary = RestApiTools.CreateFormDataBoundary();
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                // Create dictionary of request fields/values
                Dictionary<string, string> multipartFields = new Dictionary<string, string>();
                multipartFields.Add("sampleLocation", sampleLocation);
                multipartFields.Add("modelId", modelId);

                // Build the post request stream
                Stream requestStream = request.GetRequestStream();
                RestApiTools.WriteMultipartFormData(multipartFields, requestStream, boundary);
                byte[] endBytes = System.Text.Encoding.UTF8.GetBytes("--" + boundary + "--");
                requestStream.Write(endBytes, 0, endBytes.Length);
                requestStream.Close();

                // Get the prediction response
                WebResponse response = request.GetResponse();
                System.Diagnostics.Debug.WriteLine(((HttpWebResponse)response).StatusDescription);
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseFromServer = reader.ReadToEnd();
                responseFromServer = RestApiTools.JsonPrettify(responseFromServer);

                ResetChart();

                // Dynamically assign the JSON
                JObject predictionResults = JObject.Parse(responseFromServer);
                txtResults.Text = predictionResults.ToString();
                JArray probabilities = (JArray)predictionResults["probabilities"];

                dynamic probs = probabilities;
                foreach (dynamic probability in probs)
                {
                    crtResults.Series["Results"].Points.AddXY((string)probability.label, (double)probability.probability);
                }

                crtResults.Series["Results"].Sort(System.Windows.Forms.DataVisualization.Charting.PointSortOrder.Ascending, "Y");
            }
            catch (Exception Ex)
            {
                // Check the output window for any errors
                System.Diagnostics.Debug.WriteLine("General exception occurred");
                System.Diagnostics.Debug.WriteLine("Message: " + Ex.Message);
                //System.Diagnostics.Debug.WriteLine("Call stack: " + Ex.StackTrace);
            }

        }

        /// <summary>
        /// Configure the chart to graphically display the predictions.
        /// </summary>
        private void ResetChart()
        {
            crtResults.DataSource = null;
            crtResults.Series.Clear();
            crtResults.ChartAreas[0].Area3DStyle.Enable3D = false;
            crtResults.Titles.Clear();
            crtResults.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.All;

            crtResults.Series.Add("Results");
            crtResults.Series["Results"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar;
            crtResults.Series["Results"].IsVisibleInLegend = false;
            crtResults.Series["Results"].Color = Color.LightSkyBlue;
            crtResults.Series["Results"].Font = new Font("Tahoma", 8, FontStyle.Regular);
            crtResults.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            crtResults.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            crtResults.ChartAreas["ChartArea1"].IsSameFontSizeForAllAxes = true;
            crtResults.ChartAreas["ChartArea1"].AxisX.LineColor = Color.Gainsboro;
            crtResults.ChartAreas["ChartArea1"].AxisY.LineColor = Color.Gainsboro;
            crtResults.ChartAreas["ChartArea1"].AxisY.Maximum = 1;
            crtResults.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            crtResults.ChartAreas["ChartArea1"].AxisY.Interval = .1;
            crtResults.ChartAreas["ChartArea1"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap;
            crtResults.ChartAreas["ChartArea1"].BackColor = Color.Transparent;
            crtResults.BorderlineWidth = 3;
            crtResults.BorderlineColor = Color.DimGray;
            crtResults.Show();


        }
    }
}
