using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Class_类
{
    public static class XmlHelper
    {
        /// <summary>
        /// 加载指定路径的 XML 文件，并返回一个 XmlDocument 对象。
        /// </summary>
        /// <param name="xmlFilePath">XML 文件路径。</param>
        /// <returns>XmlDocument 对象。</returns>
        public static XmlDocument LoadXml(string xmlFilePath)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFilePath);
            return xml;
        }

        /// <summary>
        /// 将 XmlDocument 对象保存到指定的 XML 文件。
        /// </summary>
        /// <param name="xml">XmlDocument 对象。</param>
        /// <param name="xmlFilePath">XML 文件路径。</param>
        public static void SaveXml(XmlDocument xml, string xmlFilePath)
        {
            xml.Save(xmlFilePath);
        }

        /// <summary>
        /// 根据 XPath 表达式获取一个 XmlNode 对象。
        /// </summary>
        /// <param name="xml">XmlDocument 对象。</param>
        /// <param name="xpath">XPath 表达式。</param>
        /// <returns>XmlNode 对象。</returns>
        public static XmlNode GetXmlNode(XmlDocument xml, string xpath)
        {
            return xml.SelectSingleNode(xpath);
        }

        /// <summary>
        /// 根据 XPath 表达式获取一组 XmlNode 对象。
        /// </summary>
        /// <param name="xml">XmlDocument 对象。</param>
        /// <param name="xpath">XPath 表达式。</param>
        /// <returns>XmlNodeList 对象。</returns>
        public static XmlNodeList GetXmlNodes(XmlDocument xml, string xpath)
        {
            return xml.SelectNodes(xpath);
        }
        /// <summary>
        /// 创建xml文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="rootElementName"></param>
        public static void CreateXmlFile(string path, string rootElementName)
        {
            // 创建XML文档对象
            XmlDocument xmlDocument = new XmlDocument();

            // 添加XML文档的声明部分
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDocument.AppendChild(xmlDeclaration);

            // 创建根元素
            XmlElement rootElement = xmlDocument.CreateElement(rootElementName);
            xmlDocument.AppendChild(rootElement);

            // 保存XML文档到指定路径
            xmlDocument.Save(path);
        }
        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="path"></param>
        /// <param name="elementName"></param>
        /// <param name="elementValue"></param>
        public static void AddXmlElement(string path, string elementName, string elementValue)
        {
            // 加载XML文件
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            // 创建元素
            XmlElement element = xmlDocument.CreateElement(elementName);
            element.InnerText = elementValue;

            // 添加元素到根元素下
            xmlDocument.DocumentElement.AppendChild(element);

            // 保存XML文件
            xmlDocument.Save(path);
        }
        /// <summary>
        /// 获取元素的值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static string GetXmlElementValue(string path, string elementName)
        {
            // 加载XML文件
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            // 获取指定名称的元素
            XmlNode xmlNode = xmlDocument.SelectSingleNode($"//{elementName}");

            // 返回元素的值
            return xmlNode?.InnerText;
        }
        /// <summary>
        /// 修改元素的值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="elementName"></param>
        /// <param name="newValue"></param>
        public static void UpdateXmlElementValue(string path, string elementName, string newValue)
        {
            // 加载XML文件
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);

            // 获取指定名称的元素
            XmlNode xmlNode = xmlDocument.SelectSingleNode($"//{elementName}");

            // 更新元素的值
            xmlNode.InnerText = newValue;

            // 保存XML文件
            xmlDocument.Save(path);
        }
    }




}
