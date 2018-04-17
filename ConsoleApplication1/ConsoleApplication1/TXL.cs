using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class TXL
    {
        private HashSet<string> stopWords = new HashSet<string>();
        private Stemmer Stem = new Stemmer();

        public TXL(string stopword)
        {
            StreamReader isstop = new StreamReader(stopword);
            string line = "";

            while ((line = isstop.ReadLine()) != null)
            {
                line = toLower(line);
                if (!stopWordFound(line))
                    stopWords.Add(line);
            }
        }

        private string toLower(string s)
        {
            string str = "";
            str = s.ToLower();

            return str;
        }

        private bool stopWordFound(string s)
        {
            return stopWords.Contains(s);
        }

        public string tienXuLy(string doc)
        {
            //StreamReader ss;
            doc = toLower(doc);

            string[] words = doc.Split(' ');
            string tempLine = "";
            string tempWord = "";
            /////t sua cho nay thay cho cho foreach nha
            /////trong foreach co cho ba dung tempWord.Remove bi lap vo tan
            /////t tinh thay bang tempWord = tempWord.Remove nhung no bao loi do tempWord la cai gi trong foreach a (t quen goi no la gi roi :v)
            /////nen t thay bang vong for nay luon 
            for (int i = 0; i < words.Length; i++)
            {
                tempWord = words[i];
                if (!stopWordFound(tempWord))
                {
                    int j = 0;
                    bool bo_tu = false;
                    while (j < tempWord.Length)
                    {
                        if (!('a' <= tempWord[j] && tempWord[j] <= 'z'))
                        {
                            tempWord = tempWord.Remove(j, 1);
                            if (stopWordFound(tempWord))
                            {
                                bo_tu = true;
                                break;
                            }
                        }
                        else ++j;
                    }
                    if (!bo_tu && tempWord.Length > 0)
                    {
                        for (int k = 0; k < tempWord.Length; k++)
                            Stem.add(tempWord[k]);                            
                        Stem.stem();
                        tempWord = Stem.ToString();
                        tempLine = tempLine + tempWord + " ";
                    }
                }
            }
            
            while (tempLine.Length > 0 && tempLine[tempLine.Length - 1] == ' ')
            {            
                tempLine = tempLine.Remove(tempLine.Length - 1, 1);
            }

            return tempLine;

            //ss.str(doc); // Tao string stream tu 1 dong
            //string tempWord, tempLine = "";
            //while (ss >> tempWord) // Doc 1 tu cua dong
            //{
            //    if (!stopWordFound(tempWord))
            //    {
            //        int i = 0;
            //        bool bo_tu = false;
            //        while (i < tempWord.length())
            //        {
            //            if (!('a' <= tempWord[i] && tempWord[i] <= 'z'))
            //            {
            //                tempWord.erase(tempWord.begin() + i);
            //                if (stopWordFound(tempWord))
            //                {
            //                    bo_tu = true;
            //                    break;
            //                }
            //            }
            //            else ++i;
            //        }
            //        if (!bo_tu && tempWord.length())
            //        {
            //            Stem(tempWord);
            //            tempLine = tempLine + tempWord + " ";
            //        }
            //    }
            //}

            //    while (tempLine.length() > 0 && tempLine[tempLine.length() - 1] == ' ')
            //{
            //    tempLine.erase(tempLine.length() - 1);
            //}

            //return tempLine;
        }
    }

}