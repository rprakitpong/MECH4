%
%   MECH468/509 HW4
%   Inverted Pendulum Control
%
clear all

% System paramters
HW4_para

% Linearized state-space model around pendulum upright position
% (A,B,C,D) matrices
% x1=theta, x2=alpha, x3=thetadot, x4=alphadot
HW4_ABCD

% State feedback controller design
polevec_K = [-20 -40 -1.5-.1500j -1.500+.1500j]; %%% TASK: Design closed-loop poles
K = place(A,B,polevec_K);   

% Observer design
polevec_L = [-500 -501 -502 -503]; %%% TASK: Design observer dynamics poles
L = (place(A',C',polevec_L))';

% Servo controller design
Aaug = [A zeros(size(A,1),1);
    -C(1,:) 0];
Baug = [B; 0];
polevec_Kaug = [-15 -10 -5 -1.5-.15j -1.5+.15j]; %%% TASK: Design closed-loop poles
Kaug = place(Aaug,Baug,polevec_Kaug);
K = Kaug(1:end-1); 
Ka = Kaug(end);
