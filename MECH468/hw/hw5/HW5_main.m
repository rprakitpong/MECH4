%
%   MECH468/509 HW5
%   Inverted Pendulum Control
%
clear all

% System paramters
HW4_para

% Linearized state-space model around pendulum upright position
% (A,B,C,D) matrices
% x1=theta, x2=alpha, x3=thetadot, x4=alphadot
HW4_ABCD

% Discretization with sampling time T = 0.002 sec
T = 0.002;
sys = ss(A,B,C,D);
sysd = c2d(sys,T);

%% Discrete-time LQR controller design
%%% TASK: Design matrices Q and R
Q = [1 1 1 1; 1 1 1 1; 1 1 1 1; 1 1 1 1]; 
R = [1]; 
K = lqr(sysd,Q,R);  

%% Kalman filter design
% Add the disturbance (process noise) term "w"
% x[k+1]=Ad*x[k]+Bd*u[k]+w[k] (=Ad*x[k]+[Bd I]*[u[k];w[k]])
n = size(sysd.A,1); % # states
p = size(sysd.C,1); % # outputs
tmpB = [sysd.B eye(n)];     % Augmented B-matrix
tmpD = [sysd.D zeros(p,n)]; % Augmented D-matrix
sysd_KF = ss(sysd.A,tmpB,sysd.C,tmpD,T);

%%% TASK: Design covariances Qn, Rn
% Size of Qn (n-by-n): "n" is the number of process noises "w"
% Size of Rn (p-by-p): "p" is the number of outputs "y"
Qn = [1000]; 
Rn = [1];
[kest,L,P] = kalman(sysd_KF,Qn,Rn);

%% Discrete-time LQR servo controller design
Aaug = [sysd.A zeros(size(sysd.A,1),1);
    -sysd.C(1,:) 1];
Baug = [sysd.B; 0];
sysdaug = ss(Aaug,Baug,[sysd.C(1,:) 0],sysd.D(1),T);

%%% TASK: Design matrices Q and R
Qaug = 0.00000035*[1 1 1 1 1; 1 1 1 1 1; 1 1 1 1 1; 1 1 1 1 1; 1 1 1 1 1]; 
Raug = [1]; 
Kaug = lqr(sysdaug,Qaug,Raug); 
K = Kaug(1:end-1); Ka = Kaug(end);
