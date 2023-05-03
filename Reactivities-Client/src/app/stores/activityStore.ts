import { makeAutoObservable, runInAction } from "mobx";
import { Activity } from "../models/activity";
import agent from "../api/agent";
import { v4 as uuid } from "uuid";

export default class ActivityStore {
  activityRegistry: Map<string, Activity> = new Map<string, Activity>();
  selectedActivity: Activity | undefined = undefined;
  editMode: boolean = false;
  loading: boolean = false;
  loadingInitial: boolean = true;

  constructor() {
    makeAutoObservable(this);
  }

  get activitiesByDate() {
    return Array.from(this.activityRegistry.values()).sort(
      (a, b) => Date.parse(a.date) - Date.parse(b.date)
    );
  }

  setLoadingInitial = (value: boolean) => {
    this.loadingInitial = value;
  };

  setLoading = (value: boolean) => {
    this.loading = value;
  };

  loadActivities = async () => {
    this.setLoadingInitial(true);

    try {
      const activities = await agent.Activities.list();

      activities.forEach((a) => {
        a.date = a.date.split("T")[0];
        this.activityRegistry.set(a.id, a);
      });
    } catch (error) {
      console.log(error);
    } finally {
      this.setLoadingInitial(false);
    }
  };

  selectActivity = (id: string) => {
    this.selectedActivity = this.activityRegistry.get(id);
  };

  unselectActivity = () => {
    this.selectedActivity = undefined;
  };

  openActivityForm = (id?: string) => {
    id ? this.selectActivity(id) : this.unselectActivity();
    this.editMode = true;
  };

  closeActivityForm = () => {
    this.editMode = false;
  };

  createActivity = async (activity: Activity) => {
    this.setLoading(true);
    activity.id = uuid();

    try {
      await agent.Activities.create(activity);

      runInAction(() => {
        this.activityRegistry.set(activity.id, activity);
        this.selectedActivity = activity;
        this.editMode = false;
      });
    } catch (error) {
      console.log(error);
    } finally {
      this.setLoading(false);
    }
  };

  updateActivity = async (activity: Activity) => {
    this.setLoading(true);

    try {
      await agent.Activities.update(activity);

      runInAction(() => {
        this.activityRegistry.set(activity.id, activity);
        this.selectedActivity = activity;
        this.editMode = false;
      });
    } catch (error) {
      console.log(error);
    } finally {
      this.setLoading(false);
    }
  };

  deleteActivity = async (id: string) => {
    this.setLoading(true);

    try {
      await agent.Activities.delete(id);

      runInAction(() => {
        this.activityRegistry.delete(id);

        if (this.selectedActivity?.id === id) {
          this.unselectActivity();
        }
      });
    } catch (error) {
      console.log(error);
    } finally {
      this.setLoading(false);
    }
  };
}
