%q2b
clear all;
close all;
m = 1;
k = 1;
M = [[2*m 0 0]' [0 m 0]' [0 0 2*m]'];
K = k*[[3 -2 -1]' [-2 4 -2]' [-1 -2 3]'];
[V,w2] = eig(K,M,'vector');
V(:,:) = V(:,:) ./ V(1,:);
disp(V);
disp(w2);