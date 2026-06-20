using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Earthian.GameObject
{
    public interface IGameObject
    {
        void Draw(GameTime gameTime);
        void Update(GameTime gameTime);
        void SetTextureName(string name);
    }
}
