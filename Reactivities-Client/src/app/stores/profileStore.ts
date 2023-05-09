import { makeAutoObservable, runInAction } from "mobx";
import { Photo, Profile } from "../models/profile";
import agent from "../api/agent";
import { store } from "./store";

export default class ProfileStore {
  profile: Profile | null = null;
  loadingProfile: boolean = false;
  uploadingFile: boolean = false;
  loading: boolean = false;

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

  setUploadingFile = (value: boolean) => {
    this.uploadingFile = value;
  };

  setLoading = (value: boolean) => {
    this.loading = value;
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

  uploadPhoto = async (file: Blob) => {
    this.setUploadingFile(true);

    try {
      const response = await agent.Profiles.uploadPhoto(file);
      const photo = response.data;

      runInAction(() => {
        if (this.profile) {
          this.profile.photos?.push(photo);

          if (photo.isMain && store.userStore.user) {
            store.userStore.setImage(photo.url);
            this.profile.image = photo.url;
          }
        }
      });
    } catch (error) {
      console.log(error);
    } finally {
      this.setUploadingFile(false);
    }
  };

  setMainPhoto = async (photo: Photo) => {
    this.setLoading(true);

    try {
      await agent.Profiles.setMainPhoto(photo.id);
      store.userStore.setImage(photo.url);

      runInAction(() => {
        if (this.profile && this.profile.photos) {
          this.profile.photos.find((p) => p.isMain)!.isMain = false;
          this.profile.photos.find((p) => p.id === photo.id)!.isMain = true;
          this.profile.image = photo.url;
        }
      });
    } catch (error) {
      console.log(error);
    } finally {
      this.setLoading(false);
    }
  };

  deletePhoto = async (photo: Photo) => {
    this.setLoading(true);

    try {
      await agent.Profiles.deletePhoto(photo.id);

      runInAction(() => {
        if (this.profile) {
          this.profile.photos = this.profile.photos?.filter(
            (p) => p.id !== photo.id
          );
        }
      });
    } catch (error) {
      console.log(error);
    } finally {
      this.setLoading(false);
    }
  };
}
