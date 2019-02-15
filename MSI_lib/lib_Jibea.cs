using JiebaNet.Analyser;
using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSI_lib
{
    public class lib_Jibea
    {
        public class data_Jibea
        {
            public string key { set; get; }
            public int startIndex { set; get; }
            public int EndIndex { set; get; }
        }

        public void AddKey(string Key)
        {
           
            var segmenter = new JiebaSegmenter();
            segmenter.LoadUserDict(@"C:\jiebanet\config\saap_dict.txt");
            segmenter.AddWord(Key);
        }

        public void Delete(string Key)
        {
         
            var segmenter = new JiebaSegmenter();
            segmenter.DeleteWord(Key);
        }

        public List<data_Jibea> JiebaKey(string Key)
        {

            List<data_Jibea> lstJb = new List<data_Jibea>();

            var segmenter = new JiebaSegmenter();
         //   segmenter.LoadUserDict(@"C:\jiebanet\config\userdict.txt");
            var segments = segmenter.Cut(Key, cutAll: true);
            foreach (var o in segments)
            {
                data_Jibea jb = new data_Jibea()
                {
                    key = o,
                    startIndex = 0,
                    EndIndex = 0
                };
                lstJb.Add(jb);
            }

            segments = segmenter.Cut(Key, cutAll: true);
            //Console.WriteLine("【全模式】：{0}", string.Join("/ ", segments));

            //segments = segmenter.Cut(Key);  // 默认为精确模式
            //Console.WriteLine("【精确模式】：{0}", string.Join("/ ", segments));

            //segments = segmenter.Cut(Key);  // 默认为精确模式，同时也使用HMM模型
            //Console.WriteLine("【新词识别】：{0}", string.Join("/ ", segments));

            //segments = segmenter.CutForSearch(Key); // 搜索引擎模式
            //Console.WriteLine("【搜索引擎模式】：{0}", string.Join("/ ", segments));

            //segments = segmenter.Cut(Key);
            //Console.WriteLine("【歧义消除】：{0}", string.Join("/ ", segments));

            //Console.Read();

            return lstJb;
        }
    }
 
}
