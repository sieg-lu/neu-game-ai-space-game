using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PcgRoom {
    public int mId;

    public int mX1, mX2;
    public int mY1, mY2;

    public int mWidth;
    public int mHeight;

    public int mCenterX;
    public int mCenterY;

    public int mInterval = 3;

    /*
     * x + w
     * ^
     * |
     * |
     * +----->y + h
     */
    public void CreateRoom(int id, int x, int y, int w, int h) {
        mX1 = x;
        mX2 = x + w;
        mY1 = y;
        mY2 = y + h;
        mWidth = w;
        mHeight = h;
        mCenterX = ((mX1 + mX2) >> 1);
        mCenterY = ((mY1 + mY2) >> 1);
    }

    public bool IsIntersected(PcgRoom room) {
        PcgRoom r = new PcgRoom();
        r.CreateRoom(room.mId, room.mX1 - mInterval, room.mY1 - mInterval, room.mWidth + (mInterval << 1), room.mHeight + (mInterval << 1));
        return (mX1 < r.mX2 && mX2 > r.mX1 &&
                mY1 < r.mY2 && mY2 > r.mY1);
    }
}
