list = dir("*.mat");
allNames = {list.name};
for k = 1 : length(allNames)
   % load all data from each file
   name = allNames{k};
   data = load(name);
   time = data.output.time;
   in = data.output.CH1in;
   out = data.output.CH1out;
   sig = data.output.CH1sig;
    
   %{
   clf();
   hold on;
   title(name);
   plot(time, in);
   plot(time, out);
   plot(time, sig);
   legend('in', 'out', 'sig');
   saveas(gcf, name+".jpg");
   %}
   
   % steady state at ~7686
   % rise time = 10% to 90%
   steadystate = out(1,7686);
   [~,idx] = min(abs(out(1, 1:7686)-steadystate*0.9));
   p90 = time(1, idx);
   [~,idx] = min(abs(out(1, 1:7686)-steadystate*0.1));
   p10 = time(1, idx);
   risetime = p90 - p10;
   
   % overshoot = max - steady state
   maxVal = max(out);
   overshoot = maxVal - steadystate;
   
   % print
   disp(name);
   disp(risetime);
   disp(overshoot);
end