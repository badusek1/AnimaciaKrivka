using AnimationCurves.Enums;
using AnimationCurves.GraphicalBaseClasses;
using AnimationCurves.GraphicalClasses;
using AnimationCurves.Tools;

namespace AnimationCurves
{
    public partial class FormMain : Form
    {
        private BezierCurve bezierCurve;
        private EnumEditorState state;
        private Keys key;

        public FormMain()
        {
            InitializeComponent();

            state = EnumEditorState.Edit;

            Random rand = new();

            // bezierova krivka
            bezierCurve = new BezierCurve();

            for (int i = 0; i < 4; i++)
                bezierCurve.AddControlPoint(
                    new ControlPoint(MatrixF.BuildPointVector(((float)rand.NextDouble() * CoordTrans.xRange) + CoordTrans.xMin, ((float)rand.NextDouble() * CoordTrans.yRange) + CoordTrans.yMin)));

        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void doubleBufferPanel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            bezierCurve.Draw(g);
        }

        private void doubleBufferPanel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (bezierCurve == null)
                return;

            if (state == EnumEditorState.Edit)
            {
                int? vertexID = bezierCurve.GetVertexIDByUV(e.Location);

                if (vertexID == null)
                {
                    bezierCurve.UnselectAllVertices();
                }
                else
                {
                    if (key != Keys.ControlKey)
                        bezierCurve.UnselectAllVertices();

                    bezierCurve.SelectVertexByID(vertexID);
                }


                //if (!network.SelectNode(e.Location, ctrlPressed))
                //{
                //    network.SelectNode(new Rectangle(), ctrlPressed);
                //    state = EnumEditorState.SelectBegin;

                //    if (framedSelectionBox)
                //        SelectionBoxFramed.InitSelectionBox(e.Location);
                //    else
                //        SelectionBox.InitSelectionBox(e.Location);
                //}
            }
            else if (state == EnumEditorState.InsertNode)
            {
                MatrixF controlPoint = CoordTrans.FromUVtoXY(e.Location);

                bezierCurve.AddControlPoint(new ControlPoint(controlPoint));
            }
            else if (state == EnumEditorState.DeleteNode)
            {
                bezierCurve.Remove(bezierCurve.GetVertexIDByUV(e.Location));
            }

            doubleBufferPanel2.Invalidate();
        }


        private void doubleBufferPanel2_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void doubleBufferPanel2_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void radioButtonEdit_CheckedChanged(object sender, EventArgs e)
        {
            state = EnumEditorState.Edit;
        }

        private void radioButtonDeleteNode_CheckedChanged(object sender, EventArgs e)
        {
            state = EnumEditorState.DeleteNode;
        }

        private void radioButtonInsertNode_CheckedChanged(object sender, EventArgs e)
        {
            state = EnumEditorState.InsertNode;
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            key = e.KeyCode;
        }
    }
}
