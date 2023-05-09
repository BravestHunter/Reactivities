import { makeAutoObservable, runInAction } from "mobx";
import { Profile } from "../models/profile";
import agent from "../api/agent";

export default class ProfileStore {
  profile: Profile | null = null;
  loadingProfile: boolean = false;

  constructor() {
    makeAutoObservable(this);
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
