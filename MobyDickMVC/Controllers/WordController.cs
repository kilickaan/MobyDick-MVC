using MobyDickMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MobyDickMVC.Controllers
{
    public class WordController : Controller
    {
        // GET: Word
        public ActionResult list()
        {
            var data = new List<wordsModel>();
            data = ReturnData();

            return View(data);
        }

        public List<wordsModel> ReturnData()
        {
            XElement root = XElement.Load(Server.MapPath("~/XML/MobyDick.xml"));
            var xml = root.Elements("word")
                                  .OrderByDescending(s => (int)s.Attribute("count"))
                                  .ToArray();
            root.RemoveAll();
            foreach (XElement tab in xml)
                root.Add(tab);
            root.Save(Server.MapPath("~/XML/MobyDick2.xml"));
            string xmldata = Server.MapPath("~/XML/MobyDick2.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(xmldata);
            var wordlist = new List<wordsModel>();
            wordlist = (from rows in ds.Tables[0].AsEnumerable()
                        select new wordsModel
                        {
                            text = rows[0].ToString(),
                            count = rows[1].ToString()
                        }).Take(10).ToList();

            return wordlist;
        }
    }
}