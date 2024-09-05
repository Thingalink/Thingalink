using System.Drawing;
using System.Windows.Forms;
using Thingalink;

namespace CobbleApp
{
    public class Zone : Rectangular
    {
        protected DrawSurface _Surface;
        public DrawSurface Surface => _Surface ?? PassedParent?.Surface ?? DrawScreen.Instance;

        private ContainerZone PassedParent;
        public ContainerZone Parent => PassedParent ?? ContainerHost.Zone;
        //protected ContainerZone Parent;

        public Zone(int x, int y, int w, int h, ContainerZone parent = null, DrawSurface surface = null) : this(new Rectangle(x, y, w, h), parent, surface ?? parent?.Surface)
        {
        }
        public Zone(Rectangle rect, ContainerZone parent = null, DrawSurface surface = null) : base(rect)
        {

            PassedParent = parent;
            ParentAdd();

            _Surface = surface;
        }

        protected ListMember Item;
        protected ListMember MoveItem;
        protected ListMember KeyItem;

        public virtual void SetParent(ContainerZone parent = null)
        {
            PassedParent = parent;
        }
        public virtual void ParentAdd()
        {
            if (PassedParent == null)
                return;

            Item = Parent.Add(this);
        }
        public void MoveSub()
        {
            Movable = true;
            MoveItem = ContainerHost.Zone.AddMovable(this);
        }
        public void MovePreSub()
        {
            Movable = true;
        }
        public void KeySub()
        {
            KeyItem = ContainerHost.Zone.AddKeyable(this);
        }

        bool Movable;
        public virtual bool MoveUnSub()
        {
            if (MoveItem == null)
                return false;

            ContainerHost.Zone.RemoveMoveable(MoveItem);
            MoveItem = null;
            return true;
        }

        public virtual void ReSub()
        {
            if (Movable && MoveItem == null)
            {
                MoveSub();
            }
        }

        public void ParentRemove()
        {
            Parent?.Remove(Item);
            MoveUnSub();
            if (KeyItem != null)
            {
                ContainerHost.Zone.RemoveMoveable(KeyItem);
            }
        }

        public virtual bool KeepMove(MouseEventArgs point)
        {
            return Movable && MoveItem != null;
        }

        public virtual void Draw()
        {
            //no paint at root. paint 
        }
        public virtual void Click(MouseEventArgs point)
        {
        }
        public virtual void Move(MouseEventArgs point)
        {
        }
        public virtual void Key(Keys key)
        {
        }
        public void QueDraw()
        {
            Draw();
            //AppRoot.Instance.QueAction(Draw);
            // ContainerHost.QueUpdate(this);
        }
    }
}
