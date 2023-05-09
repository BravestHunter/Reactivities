import { makeAutoObservable, runInAction } from "mobx";
import { Profile } from "../models/profile";
import agent from "../api/agent";
import { store } from "./store";

export default class ProfileStore {
  profile: Profile | null = null;
  loadingProfile: boolean = false;

  constructor() {
    makeAutoObservable(this);
  }

  get isCurrentUser() {
    if (store.userStore.user && this.profile) {
      return store.userStore.user.username === this.profile.username;
    }

    return false;
  }

  setLoadingProfile = (value: boolean) => {
    this.loadingProfile = value;
  };

  loadProfile = async (username: string) => {
    this.setLoadingProfile(true);

    try {
      const profile = await agent.Profiles.get(username);

      runInAction(() => {
        this.profile = profile;
      });
    } catch (error) {
      console.log(error);
    } finally {
      this.setLoadingProfile(false);
    }
  };
}
