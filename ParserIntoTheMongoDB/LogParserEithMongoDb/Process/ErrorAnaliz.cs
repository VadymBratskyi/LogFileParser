using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogParserWithMongoDb.Model;

namespace LogParserWithMongoDb.Process
{
    public class ErrorAnaliz
    {
        private List<UnKnownError> unKnownErrorsList;

        private List<Error> errorsList;

        public ErrorAnaliz(List<Error> errors)
        {
            unKnownErrorsList = new List<UnKnownError>();
            errorsList = errors;
        }

        public async Task SetUnKnownError()
        {
            try
            {

                await Task.Run(() =>
                {

                    foreach (var error in errorsList)
                    {
                        
                        string s1 = "i have a car a car";
                        string s2 = "i have a new car bmw";

                        List<string> diff;
                        IEnumerable<string> set1 = s1.Split(' ').Distinct();
                        IEnumerable<string> set2 = s2.Split(' ').Distinct();

                        if (set2.Count() > set1.Count())
                        {
                            diff = set2.Except(set1).ToList();
                        }
                        else
                        {
                            diff = set1.Except(set2).ToList();
                        }


                    }

                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
