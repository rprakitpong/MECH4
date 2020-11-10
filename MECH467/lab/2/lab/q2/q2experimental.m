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
   
   % steady state at 25000
   sse = out(1, 25000) - in(1, 25000);
  
   disp(name);
   disp(sse);
end