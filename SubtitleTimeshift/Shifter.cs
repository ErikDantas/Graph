using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleTimeshift
{
    public class Shifter
    {
        async static public Task Shift(Stream input, Stream output, TimeSpan timeSpan, Encoding encoding)
        {
            string beginTime = string.Empty;
            string endTime = string.Empty;
            try
            {
                using (var outputWriter = new StreamWriter(output, encoding))
                using (var file = new StreamReader(input,encoding))
                {  
                    string lineFile = string.Empty;
                    while ((lineFile = file.ReadLine()) != null)
                    {
                        if (lineFile.Contains("-->"))
                        {
                            if(lineFile.Length > 12)
                            {
                                beginTime = lineFile.Substring(0, 12);
                                TimeSpan beginTimeUpdated = TimeSpan.Parse(beginTime);
                                beginTime = beginTimeUpdated.Add(timeSpan).ToString(@"hh\:mm\:ss\.fff");
                            }
                            if(lineFile.Length > 16)
                            {
                                endTime = lineFile.Substring(16);
                                TimeSpan endTimeUpdated = TimeSpan.Parse(endTime);
                                endTime = endTimeUpdated.Add(timeSpan).ToString(@"hh\:mm\:ss\.fff");
                            }
                            outputWriter.WriteLine(beginTime + " --> " + endTime);
                        }
                        else
                        {
                            outputWriter.WriteLine(lineFile);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
