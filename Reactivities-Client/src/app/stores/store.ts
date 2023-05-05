import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import CommonStore from "./commonStore";

interface Store {
  commonStore: CommonStore;
  activityStore: ActivityStore;
}

export const store: Store = {
  commonStore: new CommonStore(),
  activityStore: new ActivityStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
