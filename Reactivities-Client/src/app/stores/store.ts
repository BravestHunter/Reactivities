import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import CommonStore from "./commonStore";
import UserStore from "./userStore";

interface Store {
  userStore: UserStore;
  commonStore: CommonStore;
  activityStore: ActivityStore;
}

export const store: Store = {
  userStore: new UserStore(),
  commonStore: new CommonStore(),
  activityStore: new ActivityStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
