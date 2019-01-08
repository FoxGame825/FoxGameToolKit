using MS;
using System;
using System.Diagnostics;
using System.Threading;
namespace FoxGame.Timer {
	public class TimerHeap {
		private static uint NextTimerID_;
		private static uint ElapseMS_; // 1193小时的存量，对于一般游戏来说是足够用了
		private static KeyedPriorityQueue<uint, AbsTimerData, ulong> TimerQueue_;
		private static Stopwatch StopWatch_;
		private static readonly object QueueLock_;

		private TimerHeap() {
		}

		static TimerHeap() {
			TimerHeap.QueueLock_ = new object();
			TimerHeap.TimerQueue_ = new KeyedPriorityQueue<uint, AbsTimerData, ulong>();
			TimerHeap.StopWatch_ = new Stopwatch();
		}

		// 参数说明：；
		/// <summary>
		/// 添加Timer操作
		/// </summary>
		/// <param name="start">start 为启动时间(单位为毫秒)</param>
		/// <param name="interval">interval为间隔时间(单位为毫秒)，如果为0则只执行一次；</param>
		/// <param name="handler">回调函数</param>
		/// <returns>TimerID</returns>
		/// 其他几个重载函数只是多了个参数，其他一样
		public static uint AddTimer(uint start, int interval, Action handler) {
			TimerData timerData = TimerHeap.GetTimerData<TimerData>(new TimerData(), start, interval);
			timerData.ActionHandler = handler;
			return TimerHeap.AddTimer(timerData);
		}

		public static uint AddTimer<T>(uint start, int interval, Action<T> handler, T arg1) {
			TimerData<T> timerData = TimerHeap.GetTimerData<TimerData<T>>(new TimerData<T>(), start, interval);
			timerData.ActionHandler = handler;
			timerData.Arg1 = arg1;
			return TimerHeap.AddTimer(timerData);
		}

		public static uint AddTimer<T, U>(uint start, int interval, Action<T, U> handler, T arg1, U arg2) {
			TimerData<T, U> timerData = TimerHeap.GetTimerData<TimerData<T, U>>(new TimerData<T, U>(), start, interval);
			timerData.ActionHandler = handler;
			timerData.Arg1 = arg1;
			timerData.Arg2 = arg2;
			return TimerHeap.AddTimer(timerData);
		}

		public static uint AddTimer<T, U, V>(uint start, int interval, Action<T, U, V> handler, T arg1, U arg2, V arg3) {
			TimerData<T, U, V> timerData = TimerHeap.GetTimerData<TimerData<T, U, V>>(new TimerData<T, U, V>(), start, interval);
			timerData.ActionHandler = handler;
			timerData.Arg1 = arg1;
			timerData.Arg2 = arg2;
			timerData.Arg3 = arg3;
			return TimerHeap.AddTimer(timerData);
		}

		/// <summary>
		/// 删除指定的定时器
		/// </summary>
		/// <param name="timerId">TimerID</param>
		public static void DelTimer(uint timerID) {
			lock (TimerHeap.QueueLock_) {
				TimerHeap.TimerQueue_.Remove(timerID);
			}
		}

		public static void Pause() {
			TimerHeap.StopWatch_.Stop();
            //GameLog.LogError("pause");
		}

		public static void Resume() {
			TimerHeap.StopWatch_.Start();
            //GameLog.LogError("resume");
		}

		public static void Tick() {
			TimerHeap.ElapseMS_ += (uint)TimerHeap.StopWatch_.ElapsedMilliseconds;
			TimerHeap.StopWatch_.Reset();
			TimerHeap.StopWatch_.Start();
			while (TimerHeap.TimerQueue_.Count != 0) {
				lock (TimerHeap.QueueLock_) {
                    //object queueLock = TimerHeap.QueueLock_;
					AbsTimerData absTimerData = TimerHeap.TimerQueue_.Peek();
					if ((ulong)TimerHeap.ElapseMS_ < absTimerData.UnNextTimeMS) {
						break;
					}
					TimerHeap.TimerQueue_.Dequeue();

					if (absTimerData.ActionInterval > 0) {
						absTimerData.UnNextTimeMS += (ulong)((long)absTimerData.ActionInterval);
						TimerHeap.TimerQueue_.Enqueue(absTimerData.TimerID, absTimerData, absTimerData.UnNextTimeMS);
						absTimerData.DoAction();
					} else {
						absTimerData.DoAction();
					}
				}
			}
		}

		public static void Reset() {
			TimerHeap.ElapseMS_ = 0u;
			TimerHeap.NextTimerID_ = 0u;
			
			lock (TimerHeap.QueueLock_) {
				while (TimerHeap.TimerQueue_.Count != 0) {
					TimerHeap.TimerQueue_.Dequeue();
				}
			}
		}

		private static uint AddTimer(AbsTimerData p) {
			lock (TimerHeap.QueueLock_) {
				TimerHeap.TimerQueue_.Enqueue(p.TimerID, p, p.UnNextTimeMS);
				return p.TimerID;
			}			
		}

		private static T GetTimerData<T>(T p, uint start, int interval) where T : AbsTimerData {
			p.ActionInterval = interval;
			p.TimerID = (TimerHeap.NextTimerID_ += 1u);
			p.UnNextTimeMS = (ulong)(TimerHeap.ElapseMS_ + 1u + start);
			return p;
		}
	}
}
