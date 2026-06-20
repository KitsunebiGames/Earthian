#region Version History (1.0)
// 03.07.11 ~ Created
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Earthian.Utilities
{
    public class Camera
    {
        #region Fields

        protected float _zoom;
        protected Matrix _transform;
        protected Matrix _inverseTransform;
        protected Vector2 _pos;
        protected Vector2 _screenCenter;
        protected Vector2 _origin;
        protected float _rotation;
        protected Viewport _viewport;
        protected MouseState _mState;
        protected KeyboardState _keyState;
        protected Int32 _scroll;

        #endregion

        #region Properties

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        public Viewport Viewport
        {
            get { return _viewport; }
        }

        /// <summary>
        /// Camera View Matrix Property
        /// </summary>
        public Matrix Transform
        {
            get { return _transform; }
            set { _transform = value; }
        }
        /// <summary>
        /// Inverse of the view matrix, can be used to get objects screen coordinates
        /// from its object coordinates
        /// </summary>
        public Matrix InverseTransform
        {
            get { return _inverseTransform; }
        }

        public Vector2 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }

        public Vector2 centerPos
        {
            get { return _screenCenter; }
            set { _screenCenter = value; }
        }


        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        #endregion

        #region Constructor

        public Camera(Viewport viewport)
        {
            _zoom = 2.0f;
            _scroll = 1;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
            _viewport = viewport;
            _screenCenter = new Vector2(_viewport.Width / 2, _viewport.Height / 2);
            _origin = _screenCenter / _zoom;
        }

        public void ChangeViewport(Viewport viewport)
        {
            _viewport = viewport;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update the camera view
        /// </summary>
        public void Update()
        {
            //Call Camera Input
            Input();
            //Clamp zoom value
            _zoom = MathHelper.Clamp(_zoom, 0.0f, 10.0f);
            //Clamp rotation value
            _rotation = ClampAngle(_rotation);
            

            //Create view matrix
            _transform = Matrix.Identity*
                         Matrix.CreateTranslation(-_pos.X, -_pos.Y, 0) *
                         Matrix.CreateRotationZ(_rotation) *
                         Matrix.CreateTranslation(_origin.X, _origin.Y, 0) *
                         Matrix.CreateScale(new Vector3(_zoom, _zoom, 1));

            //Set the origin of the camera
            _origin = _screenCenter / _zoom;

            _screenCenter = new Vector2(_viewport.Width / 2, _viewport.Height / 2);
            
            //Update inverse matrix
            _inverseTransform = Matrix.Invert(_transform);
        }

        /// <summary>
        /// Example Input Method, rotates using cursor keys and zooms using mouse wheel
        /// </summary>
        protected virtual void Input()
        {
            /*
            float speed = 10f;
            //Check Move
            if (Earthian.Utilities.Input.IsKeyPressed(Keys.A))
            {
                _pos.X += speed;
            }
            if (Earthian.Utilities.Input.IsKeyPressed(Keys.D))
            {
                _pos.X -= speed;
            }
            if (Earthian.Utilities.Input.IsKeyPressed(Keys.W))
            {
                _pos.Y += speed;
            }
            if (Earthian.Utilities.Input.IsKeyPressed(Keys.S))
            {
                _pos.Y -= speed;
            }
             * */
        }

        /// <summary>
        /// Clamps a radian value between -pi and pi
        /// </summary>
        /// <param name="radians">angle to be clamped</param>
        /// <returns>clamped angle</returns>
        protected float ClampAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }

        #endregion
    }
}
