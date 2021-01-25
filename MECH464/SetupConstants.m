q0=[-pi/2;0] 	%Initial joint angles
u=[0;0]		%motor torques
kp=diag([1,1])	%proportional
kv=diag([2,2])	%derivative
q_d=[0;pi/2]	%desired joint angles
w0=10		%frequency for circle
qd_d=[0;0]	%desired vel...should be 0
qdd_d=[0;0]	%desired accel...should be 0