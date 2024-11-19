import { makeAutoObservable, reaction, runInAction } from 'mobx'
import { Profile } from '../models/profile'
import agent from '../api/agent'
import UserActivity from '../models/userActivity'
import Photo from '../models/photo'
import { globalStore } from './globalStore'
import ActivityStore from './activityStore'

export default class ProfileStore {
  profile: Profile | null = null
  loadingProfile: boolean = false
  uploadingFile: boolean = false
  loading: boolean = false
  followings: Profile[] = []
  loadingFollowings: boolean = false
  activeTab = 0
  userActivities: UserActivity[] = []
  loadingActivities: boolean = false

  private readonly activityStore: ActivityStore

  constructor(activityStore: ActivityStore) {
    this.activityStore = activityStore

    makeAutoObservable(this)

    reaction(
      () => this.activeTab,
      (activeTab) => {
        if (activeTab === 3 || activeTab === 4) {
          const predicate = activeTab === 3 ? 'followers' : 'following'
          this.loadFollowings(predicate)
        } else {
          this.followings = []
        }
      }
    )
  }

  setActiveTab = (activeTab: any) => {
    this.activeTab = activeTab
  }

  get isCurrentUser() {
    if (globalStore.userStore.user && this.profile) {
      return globalStore.userStore.user.username === this.profile.username
    }

    return false
  }

  setLoadingProfile = (value: boolean) => {
    this.loadingProfile = value
  }

  setUploadingFile = (value: boolean) => {
    this.uploadingFile = value
  }

  setLoading = (value: boolean) => {
    this.loading = value
  }

  setLoadingFollowings = (value: boolean) => {
    this.loadingFollowings = value
  }

  setLoadingActivities = (value: boolean) => {
    this.loadingActivities = value
  }

  loadProfile = async (username: string) => {
    this.setLoadingProfile(true)

    try {
      const profile = await agent.Profiles.get(username)

      runInAction(() => {
        this.profile = profile
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoadingProfile(false)
    }
  }

  updateProfile = async (profile: Partial<Profile>) => {
    this.setLoading(true)

    try {
      await agent.Profiles.update(profile)

      runInAction(() => {
        if (
          profile.displayName &&
          profile.displayName !== globalStore.userStore.user?.displayName
        ) {
          globalStore.userStore.setDisplayName(profile.displayName)
        }
        this.profile = { ...this.profile, ...(profile as Profile) }
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoading(false)
    }
  }

  uploadPhoto = async (file: Blob) => {
    this.setUploadingFile(true)

    try {
      const response = await agent.Profiles.uploadPhoto(file)
      const photo = response.data

      runInAction(() => {
        if (this.profile) {
          this.profile.photos?.push(photo)
        }
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setUploadingFile(false)
    }
  }

  setMainPhoto = async (photo: Photo) => {
    this.setLoading(true)

    try {
      await agent.Profiles.setMainPhoto(photo.id)
      globalStore.userStore.setPhoto(photo.url)

      runInAction(() => {
        if (this.profile && this.profile.photos) {
          this.profile.profilePhotoUrl = photo.url
        }
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoading(false)
    }
  }

  deletePhoto = async (photo: Photo) => {
    this.setLoading(true)

    try {
      await agent.Profiles.deletePhoto(photo.id)

      runInAction(() => {
        if (this.profile) {
          this.profile.photos = this.profile.photos?.filter(
            (p) => p.id !== photo.id
          )
        }
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoading(false)
    }
  }

  updateFollowing = async (username: string, following: boolean) => {
    this.setLoading(true)

    try {
      await agent.Profiles.updateFollowing(username)

      this.activityStore.updateAttendeeFollowing(username)

      runInAction(() => {
        if (
          this.profile &&
          this.profile.username !== globalStore.userStore.user?.username &&
          this.profile.username === username
        ) {
          following
            ? this.profile!.followersCount++
            : this.profile!.followersCount--
          this.profile.following = !this.profile.following
        }

        if (
          this.profile &&
          this.profile.username === globalStore.userStore.user?.username
        ) {
          this.profile.followingCount += following ? 1 : -1
        }

        this.followings.forEach((profile) => {
          if (profile.username === username) {
            profile.following
              ? profile.followersCount--
              : profile.followersCount++
            profile.following = !profile.following
          }
        })
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoading(false)
    }
  }

  loadFollowings = async (predicate: string) => {
    this.setLoadingFollowings(true)

    try {
      const followings = await agent.Profiles.listFollowings(
        this.profile!.username,
        predicate
      )

      runInAction(() => {
        this.followings = followings
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoadingFollowings(false)
    }
  }

  loadUserActivities = async (username: string, predicate?: string) => {
    this.setLoadingActivities(true)

    try {
      const activities = await agent.Profiles.listActivities(
        username,
        predicate!
      )

      runInAction(() => {
        this.userActivities = activities
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoadingActivities(false)
    }
  }
}
