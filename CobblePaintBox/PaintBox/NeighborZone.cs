using CobbleApp;
using System.Drawing;
using Thingalink;

namespace CobblePaintBox
{
    public class NeighborZone : Zone
    {
        public delegate void NeighborPass(NeighborZone cell);
        public delegate void NeighborPassFrom(NeighborZone cell, NeighborZone from);

        public SquareMatrix Matrix;

        public NeighborZone NeighborNW;
        public NeighborZone NeighborN;
        public NeighborZone NeighborNE;
        public NeighborZone NeighborW;
        public NeighborZone NeighborE;
        public NeighborZone NeighborSW;
        public NeighborZone NeighborS;
        public NeighborZone NeighborSE;

        public NeighborZone(int x, int y, int w, int h, SquareMatrix parent) : this(new Rectangle(x, y, w, h), parent)
        {

        }
        public NeighborZone(Rectangle rect, SquareMatrix parent) : base(rect, null, parent.Surface)
        {
            Matrix = parent;
        }

        public override void ParentAdd()
        {
        }

        public void NeighborDo(int radiusH, int radiusV, NeighborPassFrom action, NeighborZone from, ModRollSheet rollSheet)
        {
            int depth = radiusV - 1;//not counting self

            if (depth > 0)
            {
                NeighborUp(depth, action, from, rollSheet);

                NeighborDown(depth, action, from, rollSheet);
            }
            if (radiusH > 0)
            {
                NeighborEast(radiusH, depth, action, from, rollSheet);

                NeighborWest(radiusH, depth, action, from, rollSheet);
            }
        }
        public void NeighborUp(int depth, NeighborPassFrom action, NeighborZone from, ModRollSheet rollSheet)
        {
            rollSheet.Next();

            if (NeighborN != null)
            {

                NeighborZone acton = NeighborN;

                if (NeighborN.NeighborN != null && BumpV(rollSheet))
                {
                    if (rollSheet.Roll.Shmear)
                    {
                        action.Invoke(acton, from);
                    }

                    acton = NeighborN.NeighborN;

                    if (acton.NeighborN != null && rollSheet.Roll.BumpDouble)// RandomAccess.Percent(DrawConfig.BumpDouble.Value))
                    {

                        acton = acton.NeighborN;
                    }

                    action.Invoke(acton, from);
                }

                if (Swing(rollSheet))
                {
                    if (rollSheet.Roll.DShmear)
                    {
                        action.Invoke(acton, from);
                    }

                    if (rollSheet.Roll.SwingDir)// RandomAccess.Fifty50)
                    {
                        if (acton.NeighborE != null)
                            acton = acton.NeighborE;
                    }
                    else if (acton.NeighborW != null)
                    {
                        acton = acton.NeighborW;
                    }
                }

                action.Invoke(acton, from);
                if (depth > 1)
                {
                    acton.NeighborUp(--depth, action, from, rollSheet);
                }
            }
        }
        public void NeighborDown(int depth, NeighborPassFrom action, NeighborZone from, ModRollSheet rollSheet)
        {
            rollSheet.Next();

            if (NeighborS != null)
            {
                NeighborZone acton = NeighborS;

                if (NeighborS.NeighborS != null && BumpV(rollSheet))
                {
                    if (rollSheet.Roll.Shmear)
                    {
                        action.Invoke(acton, from);
                    }

                    acton = NeighborS.NeighborS;

                    if (acton.NeighborS != null && rollSheet.Roll.BumpDouble)//&& RandomAccess.Percent(DrawConfig.BumpDouble.Value))
                    {
                        acton = acton.NeighborS;
                    }

                    action.Invoke(acton, from);
                }

                if (Swing(rollSheet))
                {
                    if (rollSheet.Roll.DShmear)
                    {
                        action.Invoke(acton, from);
                    }

                    if (rollSheet.Roll.SwingDir)//RandomAccess.Fifty50)
                    {
                        if (acton.NeighborE != null)
                            acton = acton.NeighborE;
                    }
                    else if (acton.NeighborW != null)
                    {
                        acton = acton.NeighborW;
                    }
                }

                action.Invoke(acton, from);
                if (depth > 1)
                    acton.NeighborDown(--depth, action, from, rollSheet);
            }
        }
        public void NeighborEast(int radius, int depth, NeighborPassFrom action, NeighborZone from, ModRollSheet rollSheet)
        {
            //            var radiusThird = radius / 3.0;

            rollSheet.Next();

            if (NeighborE != null)
            {
                NeighborZone acton = NeighborE;

                if (Sway(rollSheet))
                {
                    if (rollSheet.Roll.SwingDir && acton.NeighborN != null)
                    {
                        if (rollSheet.Roll.DShmear)
                        {
                            action.Invoke(acton, from);
                        }

                        acton = acton.NeighborN;

                    }
                    else if (!rollSheet.Roll.SwingDir && acton.NeighborS != null)
                    {
                        if (rollSheet.Roll.DShmear)
                        {
                            action.Invoke(acton, from);
                        }

                        acton = acton.NeighborS;
                    }
                }

                if (NeighborE.NeighborE != null && BumpH(rollSheet))
                {
                    if (rollSheet.Roll.Shmear)
                    {
                        action.Invoke(acton, from);
                    }

                    acton = NeighborE.NeighborE;

                    if (acton.NeighborE != null && rollSheet.Roll.BumpDouble)//&& RandomAccess.Percent(DrawConfig.BumpDouble.Value))
                    {
                        acton = acton.NeighborE;
                    }

                    action.Invoke(acton, from);
                }
                //if (NeighborE.NeighborE != null && BumpH())
                //{
                //    action.Invoke(NeighborE.NeighborE, from);
                //    acton = NeighborE.NeighborE;
                //}

                if (Swing(rollSheet))
                {
                    if (rollSheet.Roll.DShmear)
                    {
                        action.Invoke(acton, from);
                    }

                    if (rollSheet.Roll.SwingDir)//RandomAccess.Fifty50) 
                    {
                        if (acton.NeighborN != null)
                            acton = acton.NeighborN;
                    }
                    else if (acton.NeighborS != null)
                    {
                        acton = acton.NeighborS;
                    }
                }

                action.Invoke(acton, from);

                if (depth > 1)
                {
                    int chunk = Ups(radius, depth); /// (int)System.Math.Round((radius / (double)(radius - depth)));
                    acton.NeighborUp(chunk, action, from, rollSheet);
                    acton.NeighborDown(chunk, action, from, rollSheet);


                }

                if (radius > 1)
                    acton.NeighborEast(--radius, --depth, action, from, rollSheet);
            }
        }
        public bool Swing(ModRollSheet rollSheet)
        {
            if (DrawConfig.Swing != null && rollSheet.Roll.Swing)// DrawConfig.Swing.Value > 0
                                                                 //                   && RandomAccess.Percent(DrawConfig.Swing.Value))
            {
                return true;
            }
            return false;
        }
        public bool Sway(ModRollSheet rollSheet)
        {
            if (DrawConfig.Sway != null && rollSheet.Roll.Sway)// DrawConfig.Swing.Value > 0
                                                               //                   && RandomAccess.Percent(DrawConfig.Swing.Value))
            {
                return true;
            }
            return false;
        }
        public bool BumpH(ModRollSheet rollSheet)
        {
            if (DrawConfig.HBump != null && rollSheet.Roll.HBump)// DrawConfig.HBump.Value > 0 
                                                                 //  && RandomAccess.Percent(DrawConfig.HBump.Value))
            {
                return true;
            }
            return false;
        }
        public bool Shmear(ModRollSheet rollSheet)
        {
            if (DrawConfig.Shmear != null && rollSheet.Roll.Shmear)// DrawConfig.VBump.Value > 0
                                                                   //    && RandomAccess.Percent(DrawConfig.VBump.Value))
            {
                return true;
            }
            return false;
        }
        public bool BumpV(ModRollSheet rollSheet)
        {
            if (DrawConfig.VBump != null && rollSheet.Roll.VBump)// DrawConfig.VBump.Value > 0
                                                                 //    && RandomAccess.Percent(DrawConfig.VBump.Value))
            {
                return true;
            }
            return false;
        }
        public int Ups(int radius, int depth)
        {
            var i = radius - depth;

            // i += (int)(radius * (1 - i / (float)radius)); 
            return depth;
        }

        public void NeighborWest(int radius, int depth, NeighborPassFrom action, NeighborZone from, ModRollSheet rollSheet)
        {
            rollSheet.Next();

            if (NeighborW != null)
            {
                NeighborZone acton = NeighborW;

                if (Sway(rollSheet))
                {
                    if (rollSheet.Roll.SwingDir && acton.NeighborN != null)
                    {
                        if (rollSheet.Roll.DShmear)
                        {
                            action.Invoke(acton, from);
                        }

                        acton = acton.NeighborN;

                    }
                    else if (!rollSheet.Roll.SwingDir && acton.NeighborS != null)
                    {
                        if (rollSheet.Roll.DShmear)
                        {
                            action.Invoke(acton, from);
                        }

                        acton = acton.NeighborS;
                    }

                }

                if (NeighborW.NeighborW != null && BumpH(rollSheet))
                {
                    if (rollSheet.Roll.Shmear)
                    {
                        action.Invoke(acton, from);
                    }

                    acton = NeighborW.NeighborW;

                    if (acton.NeighborW != null && rollSheet.Roll.BumpDouble)//&& RandomAccess.Percent(DrawConfig.BumpDouble.Value))
                    {
                        acton = acton.NeighborW;
                    }

                    action.Invoke(acton, from);
                }

                if (Swing(rollSheet))
                {
                    if (rollSheet.Roll.DShmear)
                    {
                        action.Invoke(acton, from);
                    }

                    if (rollSheet.Roll.SwingDir)//RandomAccess.Fifty50)
                    {
                        if (acton.NeighborN != null)
                            acton = acton.NeighborN;
                    }
                    else if (acton.NeighborS != null)
                    {
                        acton = acton.NeighborS;
                    }
                }

                action.Invoke(acton, from);
                if (depth > 1)
                {
                    int chunk = Ups(radius, depth);
                    acton.NeighborUp(chunk, action, from, rollSheet);
                    acton.NeighborDown(chunk, action, from, rollSheet);
                }

                if (radius > 1)
                    acton.NeighborWest(--radius, --depth, action, from, rollSheet);
            }

        }

        public Cell NeighborAnySure()
        {
            NeighborZone z = null;
            while (z == null)
            {
                z = NeighborAny();
            }
            return (Cell)z;
        }
        public NeighborZone NeighborAny()
        {
            switch (RandomAccess.Next(0, 8))
            {
                case 0:
                    return NeighborNW;
                case 1:
                    return NeighborN;
                case 2:
                    return NeighborNE;
                case 3:
                    return NeighborW;
                case 4:
                    return NeighborE;
                case 5:
                    return NeighborSW;
                case 6:
                    return NeighborS;
                case 7:
                    return NeighborSE;
                default:
                    return null;
            }
        }

        public delegate Cell CellSelector(Cell cell);

        public static Cell SelectSure(CellSelector method, Cell cell, Cell not = null)
        {
            Cell z = null;
            while (z == null && (not == null || z != not))
            {
                z = method(cell);
            }
            return z;
        }
        public static Cell NeighborAnySureReach(Cell cell, Cell not = null)
        {
            return SelectSure(NeighborAnyReach, cell, not);
        }
        public static Cell NeighborAnyReach(Cell cell)
        {
            switch (RandomAccess.Next(0, 8))
            {
                case 0:
                    return cell.NeighborNW?.NeighborNW;
                case 1:
                    return cell.NeighborN?.NeighborN;
                case 2:
                    return cell.NeighborNE?.NeighborNE;
                case 3:
                    return cell.NeighborW?.NeighborW;
                case 4:
                    return cell.NeighborE?.NeighborE;
                case 5:
                    return cell.NeighborSW?.NeighborSW;
                case 6:
                    return cell.NeighborS?.NeighborS;
                case 7:
                    return cell.NeighborSE?.NeighborSE;
                default:
                    return null;
            }
        }


        public static Cell NeighborNSureReach(Cell cell, Cell not = null)
        {
            return SelectSure(NeighborNReach, cell, not);
        }
        protected static Cell NeighborNReach(Cell cell)
        {
            switch (RandomAccess.Next(0, 3))
            {
                case 0:
                    return cell.NeighborNW?.NeighborNW;
                case 1:
                    return cell.NeighborN?.NeighborN;
                case 2:
                    return cell.NeighborNE?.NeighborNE;
                default:
                    return null;
            }
        }

        public static Cell NeighborNWSureReach(Cell cell, Cell not = null)
        {
            return SelectSure(NeighborNWReach, cell, not);
        }
        protected static Cell NeighborNWReach(Cell cell)
        {
            switch (RandomAccess.Next(0, 3))
            {
                case 0:
                    return cell.NeighborNW?.NeighborNW;
                case 1:
                    return cell.NeighborW?.NeighborW;
                case 2:
                    return cell.NeighborN?.NeighborN;
                default:
                    return null;
            }
        }

        public static Cell NeighborNESureReach(Cell cell, Cell not = null)
        {
            return SelectSure(NeighborNEReach, cell, not);
        }
        protected static Cell NeighborNEReach(Cell cell)
        {
            switch (RandomAccess.Next(0, 3))
            {
                case 0:
                    return cell.NeighborNE?.NeighborNE;
                case 1:
                    return cell.NeighborE?.NeighborE;
                case 2:
                    return cell.NeighborN?.NeighborN;
                default:
                    return null;
            }
        }


        public static Cell NeighborEastSureReach(Cell cell, Cell not = null)
        {
            return SelectSure(NeighborEastReach, cell, not);
        }
        protected static Cell NeighborEastReach(Cell cell)
        {
            switch (RandomAccess.Next(0, 3))
            {
                case 0:
                    return cell.NeighborNE?.NeighborNE;
                case 1:
                    return cell.NeighborE?.NeighborE;
                case 2:
                    return cell.NeighborSE?.NeighborSE;
                default:
                    return null;
            }
        }
        public static Cell NeighborSouthSureReach(Cell cell, Cell not = null)
        {
            return SelectSure(NeighborSReach, cell, not);
        }
        public static Cell NeighborSReach(Cell cell)
        {
            switch (RandomAccess.Next(0, 3))
            {
                case 0:
                    return cell.NeighborSW?.NeighborSW;
                case 1:
                    return cell.NeighborS?.NeighborS;
                case 2:
                    return cell.NeighborSE?.NeighborSE;
                default:
                    return null;
            }
        }
        public static Cell NeighborSESureReach(Cell cell, Cell not = null)
        {
            return SelectSure(NeighborSEReach, cell, not);
        }
        public static Cell NeighborSEReach(Cell cell)
        {
            switch (RandomAccess.Next(0, 3))
            {
                case 0:
                    return cell.NeighborE?.NeighborE;
                case 1:
                    return cell.NeighborS?.NeighborS;
                case 2:
                    return cell.NeighborSE?.NeighborSE;
                default:
                    return null;
            }
        }
        public static Cell NeighborSWSureReach(Cell cell, Cell not = null)
        {
            return SelectSure(NeighborSWReach, cell, not);
        }
        public static Cell NeighborSWReach(Cell cell)
        {
            switch (RandomAccess.Next(0, 3))
            {
                case 0:
                    return cell.NeighborW?.NeighborW;
                case 1:
                    return cell.NeighborS?.NeighborS;
                case 2:
                    return cell.NeighborSW?.NeighborSW;
                default:
                    return null;
            }
        }

        public static void RowNeighbor(NeighborZone northCell, NeighborZone newCell)
        {

            northCell.NeighborS = newCell;
            newCell.NeighborN = northCell;

            if (northCell.NeighborW != null)
            {
                northCell.NeighborW.NeighborSE = newCell;

                northCell.NeighborW.NeighborS.NeighborE = newCell;

                newCell.NeighborNW = northCell.NeighborW;
                newCell.NeighborW = northCell.NeighborW.NeighborS;
            }

            if (northCell.NeighborE != null)
            {
                northCell.NeighborE.NeighborSW = newCell;

                //c.NeighborE.NeighborS.NeighborW = newC;

                newCell.NeighborNE = northCell.NeighborE;
            }
        }

    }
}
