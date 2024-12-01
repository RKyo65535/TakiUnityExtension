using System;
using UnityEngine;

namespace TakiExtensions.TakiExtension.KaleidScopedPosition
{
    public static class KaleidScopedPosition
    {
        static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
        }

        public static bool IsInTriangle(this Vector3 original, float oneSide)
        {

            Vector2 v1 = new Vector2(0, 0.57735f * oneSide);
            Vector2 v2 = new Vector2(oneSide/2, -0.288675f * oneSide);
            Vector2 v3 = new Vector2(-oneSide/2, -0.288675f * oneSide);

            bool b1, b2, b3;

            b1 = Sign(original, v1, v2) < 0.0f;
            b2 = Sign(original, v2, v3) < 0.0f;
            b3 = Sign(original, v3, v1) < 0.0f;

            return ((b1 == b2) && (b2 == b3));
        }

        public static Vector3 GetKaleidScopedPosition(this Vector3 original,int x,int y,float oneSide)
        {
            // 上下反転が入る条件
            bool isFlipY = MathF.Abs(x + y) % 2 == 1;
            int YScale = isFlipY ? -1 : 1;
            float moveY = isFlipY ? 0.288675f * oneSide : 0;

            // 回転する角度
            float rotate = MathF.Sign(x)* MathF.Abs(x) % 3 * 120;

            // 移動量
            Vector3 T = new Vector3(x * oneSide / 2, y * 0.866f * oneSide + moveY, 0);
            Matrix4x4 T4 = Matrix4x4.Translate(T);
            //回転量
            Quaternion R = Quaternion.Euler(0, 0, rotate);
            Matrix4x4 R4 = Matrix4x4.Rotate(R);
            //拡大縮小
            Vector3 S = new Vector3(1, YScale, 1);
            Matrix4x4 S4 = Matrix4x4.Scale(S);



            // アフィン変換用行列の作成
            Matrix4x4 affine = T4  * S4 * R4;

            return affine.MultiplyPoint(original);


        }


    }
}
