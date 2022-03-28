using Microsoft.VisualStudio.TestTools.UnitTesting;
using CimPointConv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CimPointConv.Tests
{
    [TestClass()]
    public class IgnitionTagTests
    {
        [TestMethod()]
        public void PrepareNodeTest()
        {
            Element node;
            node = IgnitionTag.PrepareNode("POINT1 * (POINT2 + ABS(POINT3)) GT 8");
            Assert.AreEqual("POINT1 * [0] > 8", node.Expression);
            Assert.AreEqual(1, node.Nodes.Count);
            Assert.AreEqual("POINT2 + abs([0])", node.Nodes[0].Expression);
            Assert.AreEqual(1, node.Nodes[0].Nodes.Count);
            Assert.AreEqual("POINT3", node.Nodes[0].Nodes[0].Expression);
            Assert.AreEqual(0, node.Nodes[0].Nodes[0].Nodes.Count);
            
            node = IgnitionTag.PrepareNode("POINT1*(POINT2+ABS(POINT3))GT(POINT4+1)");
            Assert.AreEqual("POINT1*[0]>[1]", node.Expression);
            Assert.AreEqual(2, node.Nodes.Count);
            Assert.AreEqual("POINT2+abs([0])", node.Nodes[0].Expression);
            Assert.AreEqual("POINT4+1", node.Nodes[1].Expression);
            Assert.AreEqual(1, node.Nodes[0].Nodes.Count);            
            Assert.AreEqual("POINT3", node.Nodes[0].Nodes[0].Expression);
            Assert.AreEqual(0, node.Nodes[0].Nodes[0].Nodes.Count);

            node = IgnitionTag.PrepareNode("POINT1*(POINT2+ABS(POINT3))GT(POINT4+1) AND (POINT5 NE POINT6 OR POINT5 EQ POINT7)");
            Assert.AreEqual("POINT1*[0]>[1] && [2]", node.Expression);
            Assert.AreEqual(3, node.Nodes.Count);
            Assert.AreEqual("POINT2+abs([0])", node.Nodes[0].Expression);
            Assert.AreEqual("POINT4+1", node.Nodes[1].Expression);
            Assert.AreEqual("POINT5 != POINT6 || POINT5 == POINT7", node.Nodes[2].Expression);
            Assert.AreEqual(1, node.Nodes[0].Nodes.Count);
            Assert.AreEqual("POINT3", node.Nodes[0].Nodes[0].Expression);
            Assert.AreEqual(0, node.Nodes[0].Nodes[0].Nodes.Count);

            node = IgnitionTag.PrepareNode("POINT1 AND POINT2 AND POINT3 OR POINT4");
            Assert.AreEqual("POINT1 && POINT2 && POINT3 || POINT4", node.Expression);
            Assert.AreEqual(0, node.Nodes.Count);

            node = IgnitionTag.PrepareNode("POINT1[12] BAND (POINT2+POINT3)");
            Assert.AreEqual("POINT1\\[12\\] & [0]", node.Expression);
            Assert.AreEqual(1, node.Nodes.Count);
        }

        [TestMethod()]
        public void EvalExpressionTest()
        {
            string expression;
            
            expression = IgnitionTag.EvalExpression("POINT BAND 1", null);
            Assert.AreEqual("{[.]POINT} & 1", expression);

            expression = IgnitionTag.EvalExpression("POINT1 * (POINT2 + ABS(POINT3))", null);
            Assert.AreEqual("{[.]POINT1} * ({[.]POINT2} + abs({[.]POINT3}))", expression);

            expression = IgnitionTag.EvalExpression("POINT1[12] BAND (POINT2+POINT3)", null);
            Assert.AreEqual("{[.]POINT1[12]} & ({[.]POINT2}+{[.]POINT3})", expression);
        }
    }
}