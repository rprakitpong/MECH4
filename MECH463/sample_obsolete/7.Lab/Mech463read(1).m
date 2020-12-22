%% Program to read .mat files saved by Logger for MECH 463 Experiment
%% SP, 2012
clear all;close all;clc
filepath=pwd;
separator='/';
[fname pname]=uigetfile([filepath separator '*.mat']);

load([pname fname]);

dt=1/freq;
taxis=0:dt:dt*(buflen-1);

figure(1)
plot(taxis,indata(:,1),'r') 
hold on

plot(taxis,indata(:,2),'b')
%% Only plot the raw data, exclude channel 3 with the filtered data
xlabel('Time (s)','FontSize',20)
ylabel('Signal (V)','FontSize',20)
set(gca,'LineWidth',2,'FontSize',16,'Box','off')
h=legend(' Tachometer Reading','Acceleration');
set(h,'FontSize',14);

axis square
set(gcf,'Color','none')
%export_fig time_series.pdf

figure(2)
fs=freq;
ts=length(indata)/fs;
t_i=floor(0.3*ts);
t_f=floor(0.7*ts);
n_i=ceil(t_i*fs);
n_f=ceil(t_f*fs);
tchunk=t_i:1/fs:t_f;
if(n_i==0)
    n_i=1;
end

Accln_chunk=(indata(n_i:n_f,3));
Force_chunk=(indata(n_i:n_f,1));
 
clf;
plot(tchunk,Force_chunk*1e-3,'r','Linewidth',2)
hold on
plot(tchunk,Accln_chunk,'b','Linewidth',2)
title('MECH 364: Shaky Table Data','FontSize',14)
xlabel('Time (s)','FontSize',14)
ylabel('Acceleration (V)','FontSize',14)
h=legend('Scaled Tachometer Reading','Filtered Acceleration');
set(h,'FontSize',14);
set(gca,'FontSize',16,'LineWidth',2)
set(gca,'LineWidth',2,'FontSize',16,'Box','off')
axis square
set(gcf,'Color','none')
%export_fig selected.pdf