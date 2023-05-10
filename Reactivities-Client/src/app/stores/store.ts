import { createContext, useContext } from "react";
import ActivityStore from "./activityStore";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ModalStore from "./modalStore";
import ProfileStore from "./profileStore";
import CommentStore from "./commentsStore";

interface Store {
  userStore: UserStore;
  profileStore: ProfileStore;
  commonStore: CommonStore;
  activityStore: ActivityStore;
  modalStore: ModalStore;
  commentStore: CommentStore;
}

export const store: Store = {
  userStore: new UserStore(),
  profileStore: new ProfileStore(),
  commonStore: new CommonStore(),
  activityStore: new ActivityStore(),
  modalStore: new ModalStore(),
  commentStore: new CommentStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
  return useContext(StoreContext);
}
